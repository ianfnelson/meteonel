import datetime
import time
import sys
import json
import pika
import sensor_bme280

DEVICE_ID = "leekpi"
VHOST = "meteonel_dev"
QUEUE = "bme280_persisted"
CREDENTIALS = "meteonel:7Z*0f4QRHOuO"

node1 = pika.URLParameters('amqp://'+CREDENTIALS+'@nelsonnas/' + VHOST)
node2 = pika.URLParameters('amqp://'+CREDENTIALS+'@steakpi/' + VHOST)
node3 = pika.URLParameters('amqp://'+CREDENTIALS+'@minecraft/' + VHOST)
node4 = pika.URLParameters('amqp://'+CREDENTIALS+'@leekpi/' + VHOST)
all_nodes = [node1, node2, node3, node4]

def bme280_node_run():

    while True:
        try:
            time.sleep(60 - time.time() % 60)
            humidity, pressure, temperature = sensor_bme280.read_all()

            data = {}
            data['device'] = DEVICE_ID
            data['timestamp'] = datetime.datetime.utcnow().isoformat() + 'Z'
            data['tempAmbient'] = temperature
            data['humidity'] = humidity
            data['pressure'] = pressure

            message = json.dumps(data)

            try:
                connection = pika.BlockingConnection(all_nodes)
                channel = connection.channel()
                channel.basic_publish('',QUEUE,message,
                    pika.BasicProperties(content_type='text/json', delivery_mode=2))
            except pika.exceptions.AMQPConnectionError:
                continue

        except KeyboardInterrupt:
            connection.close()
            break

if __name__ == '__main__':
    bme280_node_run()
