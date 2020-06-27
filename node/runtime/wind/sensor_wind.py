from gpiozero import Button
from gpiozero import MCP3008
import time
import statistics
import math

adc = MCP3008(channel=0)
store_directions = []
store_speeds = []
wind_speed_sensor = Button(5)
wind_count = 0

volts = {0.4: 0.0,
         1.4: 22.5,
         1.2: 45.0,
         2.8: 67.5,
         2.7: 90.0,
         2.9: 112.5,
         2.2: 135.0,
         2.5: 157.5,
         1.8: 180.0,
         2.0: 202.5,
         0.7: 225.0,
         0.8: 247.5,
         0.1: 270.0,
         0.3: 292.5,
         0.2: 315.0,
         0.6: 337.5}

def get_average_angle(angles):
    sin_sum = 0.0
    cos_sum = 0.0

    for angle in angles:
        r = math.radians(angle)
        sin_sum += math.sin(r)
        cos_sum += math.cos(r)

    flen = float(len(angles))
    s = sin_sum / flen
    c = cos_sum / flen
    arc = math.degrees(math.atan(s / c))
    average = 0.0

    if s > 0 and c > 0:
        average = arc
    elif c < 0:
        average = arc + 180
    elif s < 0 and c > 0:
        average = arc + 360

    return 0.0 if average == 360 else average

def reset_wind_count():
    global wind_count
    wind_count = 0

def spin():
    global wind_count
    wind_count = wind_count + 1

wind_speed_sensor.when_pressed = spin

def getreadings(mean_interval, gust_interval):
    mean_start_time = time.time()
    while time.time() - mean_start_time <= mean_interval:
        gust_start_time = time.time()
        while time.time() - gust_start_time <= gust_interval:
            time.sleep(gust_interval)
            voltage = round(adc.value*3.3,1)
            if voltage in volts:
                store_directions.append(volts[voltage])
            store_speeds.append(1.492 * wind_count / gust_interval)
            reset_wind_count()
    wind_speed = statistics.mean(store_speeds)
    wind_gust = max(store_speeds)
    wind_direction = get_average_angle(store_directions)
    return round(wind_speed,2), round(wind_gust,2), round(wind_direction,0)

if __name__ == "__main__":
    while True:
        store_speeds = []
        wind_speed, wind_gust, wind_direction = getreadings(30,5)
        print(wind_speed, wind_gust, wind_direction)

