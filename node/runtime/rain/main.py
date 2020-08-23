from gpiozero import Button
import datetime
import time
import sys
import json
import os
import pika

DEVICE_ID = os.environ.get('METEONEL_DEVICE_ID')
VHOST = "meteonel"
QUEUE = "raintip"
CREDENTIALS = "meteonel:7Z*0f4QRHOuO"

node = pika.URLParameters('amqp://'+CREDENTIALS+'@nelsonnas/' + VHOST)

rain_sensor = Button(6)
BUCKET_SIZE = 0.2794

def bucket_tipped():
    data = {}
    data['device'] = DEVICE_ID
    data['timestamp'] = datetime.datetime.utcnow().isoformat() + 'Z'
    data['rain'] = BUCKET_SIZE

    message = json.dumps(data)

    connection = pika.BlockingConnection(node)
    channel = connection.channel()
    channel.basic_publish('',QUEUE,message,
        pika.BasicProperties(content_type='text/json', delivery_mode=2))
    connection.close()

rain_sensor.when_pressed = bucket_tipped

while True:
    time.sleep(60)
