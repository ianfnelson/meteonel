import datetime
import time
import sys
import json
import os
import pika
import sensor_ds18b20

DEVICE_ID = os.environ.get('METEONEL_DEVICE_ID')
VHOST = "meteonel"
QUEUE = "ds18b20"
CREDENTIALS = "meteonel:7Z*0f4QRHOuO"

temp_probe = sensor_ds18b20.DS18B20()

def ds18b20_node_run():
    node = pika.URLParameters('amqp://'+CREDENTIALS+'@nelsonnas/' + VHOST)
    connection = pika.BlockingConnection(node)
    channel = connection.channel()

    while True:
        time.sleep(60 - time.time() % 60)
        temperature = temp_probe.read_temp()

        data = {}
        data['device'] = DEVICE_ID
        data['timestamp'] = datetime.datetime.utcnow().isoformat() + 'Z'
        data['tempGround'] = temperature

        message = json.dumps(data)

        channel.basic_publish('',QUEUE,message,
            pika.BasicProperties(content_type='text/json', delivery_mode=2))

if __name__ == '__main__':
    ds18b20_node_run()
