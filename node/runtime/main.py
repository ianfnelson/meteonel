import datetime
import time
import sys
import json
import iothub_client
import bme280_sensor
import ds18b20_sensor
# pylint: disable=E0611
from iothub_client import IoTHubClient, IoTHubClientError, IoTHubTransportProvider, IoTHubClientResult
from iothub_client import IoTHubMessage, IoTHubMessageDispositionResult, IoTHubError, DeviceMethodReturnValue

# TODO - move this stuff to settings class
CONNECTION_STRING = "HostName=meteonel-hub-dev.azure-devices.net;DeviceId=office1;SharedAccessKey=ZC6ewYTaqGhLZDfNaUC9qSOsfw7fXZ0RTx2vtjNOPzU="
PROTOCOL = IoTHubTransportProvider.MQTT
MESSAGE_TIMEOUT = 10000
DEVICE_ID = "office1"

def send_confirmation_callback(message, result, user_context):
    print ( "IoT Hub responded to message with status: %s" % (result) )

def iothub_client_init():
    # Create an IoT Hub client
    client = IoTHubClient(CONNECTION_STRING, PROTOCOL)
    return client

def weather_node_run():

    try:
        client = iothub_client_init()
        print ( "IoT Hub device sending periodic messages, press Ctrl-C to exit" )

        while True:

            humidity, pressure, temperature = bme280_sensor.read_all()

            # TODO - optionally read ds18b20 data

            data = {}
            data['timestamp'] = datetime.datetime.utcnow().isoformat() + 'Z'
            data['device_id'] = DEVICE_ID
            data['sensor_type'] = 'temperature'
            data['temperature_ambient_celsius'] = temperature
            data['humidity_relative_percentage'] = humidity
            data['pressure_millibars'] = pressure

            json_data = json.dumps(data)
                        
            message = IoTHubMessage(json_data)

            # Send the message.
            print( "Sending message: %s" % message.get_string() )
            client.send_event_async(message, send_confirmation_callback, None)
            
            time.sleep(4.9)

    except IoTHubError as iothub_error:
        print ( "Unexpected error %s from IoTHub" % iothub_error )
        return
    except KeyboardInterrupt:
        print ( "IoTHubClient sample stopped" )

if __name__ == '__main__':
    weather_node_run()
