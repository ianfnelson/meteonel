import datetime
import time
import sys
import json
import os
import pika
import sensor_ds18b20

DEVICE_ID = os.environ.get('METEONEL_DEVICE_ID')
VHOST = "meteonel"
QUEUE = "ds18b20_persisted"
CREDENTIALS = "meteonel:7Z*0f4QRHOuO"

node1 = pika.URLParameters('amqp://'+CREDENTIALS+'@nelsonnas/' + VHOST)
node2 = pika.URLParameters('amqp://'+CREDENTIALS+'@steakpi/' + VHOST)
node3 = pika.URLParameters('amqp://'+CREDENTIALS+'@minecraft/' + VHOST)
node4 = pika.URLParameters('amqp://'+CREDENTIALS+'@leekpi/' + VHOST)
node5 = pika.URLParameters('amqp://'+CREDENTIALS+'@pumpkinpi/' + VHOST)
all_nodes = [node1, node2, node3, node4, node5]

temp_probe = sensor_ds18b20.DS18B20()

def ds18b20_node_run():

    while True:
        try:
            time.sleep(60 - time.time() % 60)
            temperature = temp_probe.read_temp()

            data = {}
            data['device'] = DEVICE_ID
            data['timestamp'] = datetime.datetime.utcnow().isoformat() + 'Z'
            data['tempGround'] = temperature

            message = json.dumps(data)

            try:
                connection = pika.BlockingConnection(all_nodes)
                channel = connection.channel()
                channel.basic_publish('',QUEUE,message,
                    pika.BasicProperties(content_type='text/json', delivery_mode=2))
                connection.close()
            except pika.exceptions.AMQPConnectionError:
                continue

        except KeyboardInterrupt:
            connection.close()
            break

if __name__ == '__main__':
    ds18b20_node_run()
