import json
from pijuice import PiJuice
from time import sleep

pijuice = PiJuice(1, 0x14)

def read_all():
    status = pijuice.status.GetStatus()['data']
    batteryStatus = status['battery']
    powerInput = status['powerInput']
    chargeLevel = pijuice.status.GetChargeLevel()['data']
    batteryTemperature = pijuice.status.GetBatteryTemperature()['data']
    return batteryStatus, powerInput, chargeLevel, batteryTemperature

if __name__ == "__main__":
    while True:
        batteryStatus, powerInput, chargeLevel, batteryTemperature = read_all()
        print(batteryStatus, powerInput, chargeLevel, batteryTemperature)
        sleep(5)

