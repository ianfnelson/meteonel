using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using Meteonel.Ingestor.DomainModel;
using NHibernate;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Meteonel.Ingestor
{
    class Program
    {
        private static readonly ISessionFactory _sessionFactory = new FactoryManager().Instance;
        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);
        
        static void Main(string[] args)
        {
            var devices = GetDevices();

            var factory = new ConnectionFactory
            {
                HostName="192.168.1.97",
                UserName="meteonel",
                Password = "7Z*0f4QRHOuO",
                VirtualHost = "meteonel"
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "bme280_persisted",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: new Dictionary<string, object>() {{"x-queue-type", "quorum"}});
                    
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var bme280Message = JsonSerializer.Deserialize<Bme280Message>(message);

                        var bme280Reading = new Bme280Reading
                        {
                            Device = devices.Single(x => x.Name == bme280Message.Device),
                            TimeStamp = bme280Message.Timestamp,
                            Humidity = bme280Message.Humidity,
                            Pressure = bme280Message.Pressure,
                            TempAmbient = bme280Message.TempAmbient
                        };

                        using (var session = _sessionFactory.OpenSession())
                        {
                            try
                            {
                                session.Save(bme280Reading);
                                session.Flush();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                            finally
                            {
                                channel.BasicAck(ea.DeliveryTag, false);
                            }
                        }
                    };

                    channel.BasicConsume(queue: "bme280_persisted", autoAck: false, consumer: consumer);
                    
                    Console.CancelKeyPress += new ConsoleCancelEventHandler(OnExit);
                    _closing.WaitOne();
                }
            }
        }

        protected static void OnExit(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("Exit");
            _closing.Set();
        }

        public static IList<Device> GetDevices()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var devices = session.Query<Device>().ToList();
                return devices;
            }
        }
    }
}