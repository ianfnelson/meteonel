FROM python:3

RUN apt-get update
RUN apt-get install i2c-tools -y

RUN pip3 install RPi.bme280
RUN pip3 install smbus2
RUN pip3 install pika


COPY . .

CMD [ "python", "./main.py" ]