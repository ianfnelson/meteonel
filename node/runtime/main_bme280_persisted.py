import datetime
import time
import sys
import json
import pika
import sensor_bme280

DEVICE_ID = "leekpi"
VHOST = "meteonel_dev"

def bme280_node_run():

    try:

        while True:

            humidity, pressure, temperature = sensor_bme280.read_all()

            data = {}
            data['device'] = DEVICE_ID
            data['timestamp'] = datetime.datetime.utcnow().isoformat() + 'Z'
            data['tempAmbient'] = temperature
            data['humidity'] = humidity
            data['pressure'] = pressure

            json_data = json.dumps(data)

            print( "Sending message: %s" % json_data )

    except KeyboardInterrupt:
        print ( "Stopped." )

if __name__ == '__main__':
    bme280_node_run()
