from gpiozero import Button
import datetime
import time
import sys
import json
import os
import pika

DEVICE_ID = os.environ.get('METEONEL_DEVICE_ID')
VHOST = "meteonel"
QUEUE = "raintip_persisted"
CREDENTIALS = "meteonel:7Z*0f4QRHOuO"

node1 = pika.URLParameters('amqp://'+CREDENTIALS+'@nelsonnas/' + VHOST)
node2 = pika.URLParameters('amqp://'+CREDENTIALS+'@steakpi/' + VHOST)
node3 = pika.URLParameters('amqp://'+CREDENTIALS+'@minecraft/' + VHOST)
node4 = pika.URLParameters('amqp://'+CREDENTIALS+'@leekpi/' + VHOST)
node5 = pika.URLParameters('amqp://'+CREDENTIALS+'@pumpkinpi/' + VHOST)
all_nodes = [node1, node2, node3, node4, node5]

rain_sensor = Button(6)
BUCKET_SIZE = 0.2794

def bucket_tipped():
    data = {}
    data['device'] = DEVICE_ID
    data['timestamp'] = datetime.datetime.utcnow().isoformat() + 'Z'
    data['rain'] = BUCKET_SIZE

    message = json.dumps(data)

    connection = pika.BlockingConnection(all_nodes)
    channel = connection.channel()
    channel.basic_publish('',QUEUE,message,
        pika.BasicProperties(content_type='text/json', delivery_mode=2))
    connection.close()

rain_sensor.when_pressed = bucket_tipped

while True:
    time.sleep(60)
