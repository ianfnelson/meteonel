import datetime
import time
import sys
import json
import os
import pika
import sensor_wind

DEVICE_ID = os.environ.get('METEONEL_DEVICE_ID')
VHOST = "meteonel"
QUEUE = "wind_persisted"
CREDENTIALS = "meteonel:7Z*0f4QRHOuO"

node1 = pika.URLParameters('amqp://'+CREDENTIALS+'@nelsonnas/' + VHOST)
node2 = pika.URLParameters('amqp://'+CREDENTIALS+'@steakpi/' + VHOST)
node3 = pika.URLParameters('amqp://'+CREDENTIALS+'@minecraft/' + VHOST)
node4 = pika.URLParameters('amqp://'+CREDENTIALS+'@leekpi/' + VHOST)
node5 = pika.URLParameters('amqp://'+CREDENTIALS+'@pumpkinpi/' + VHOST)
all_nodes = [node1, node2, node3, node4, node5]

def wind_run():

    while True:
        try:
            time.sleep(120 - time.time() % 120)

            windspeed, windgust, winddirection = sensor_wind.getreadings(110, 10)

            data = {}
            data['device'] = DEVICE_ID
            data['timestamp'] = datetime.datetime.utcnow().isoformat() + 'Z'
            data['windSpeed'] = windspeed
            data['windGust'] = windgust
            data['windDirection'] = winddirection

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
    wind_run()
