import datetime
import time
import sys
import json
import os
import pika
import sensor_bme280

DEVICE_ID = os.environ.get('METEONEL_DEVICE_ID')
VHOST = "meteonel"
QUEUE = "bme280"
CREDENTIALS = "meteonel:7Z*0f4QRHOuO"

def bme280_node_run():

    node = pika.URLParameters('amqp://'+CREDENTIALS+'@nelsonnas/' + VHOST)
    connection = pika.BlockingConnection(node)
    channel = connection.channel()

    while True:
        time.sleep(60 - time.time() % 60)
        humidity, pressure, temperature = sensor_bme280.read_all()

        data = {}
        data['device'] = DEVICE_ID
        data['timestamp'] = datetime.datetime.utcnow().isoformat() + 'Z'
        data['tempAmbient'] = temperature
        data['humidity'] = humidity
        data['pressure'] = pressure

        message = json.dumps(data)

        channel.basic_publish('',QUEUE,message,
            pika.BasicProperties(content_type='text/json', delivery_mode=2))

if __name__ == '__main__':
    bme280_node_run()
