using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using Meteonel;
using Meteonel.Aggregator.Aggregators;
using NHibernate;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Aggregator
{
    class Program
    {
        private static readonly ISessionFactory _sessionFactory = new FactoryManager().Instance;
        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();

            var factory = new ConnectionFactory
            {
                HostName = "192.168.1.97",
                UserName = "meteonel",
                Password = "7Z*0f4QRHOuO",
                VirtualHost = "meteonel"
            };

            using (var connection = factory.CreateConnection())
            {
                var bme280Aggregator = new Bme280Aggregator(_sessionFactory);

                var aggregators = new List<IAggregator>
                {
                    bme280Aggregator
                };

                var incomingChannel = connection.CreateModel();
                incomingChannel.QueueDeclare("aggregations", true, false, false);

                var consumer = new EventingBasicConsumer(incomingChannel);

                consumer.Received += (model, ea) =>
                {
                    try
                    {
                        var bodyBytes = ea.Body;
                        var bodyString = Encoding.UTF8.GetString(bodyBytes);
                        var message = JsonSerializer.Deserialize<AggregationMessage>(bodyString);

                        var aggregator = aggregators.SingleOrDefault(x => x.SensorType == message.SensorType);
                        aggregator?.Aggregate(message.Device);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    finally
                    {
                        incomingChannel.BasicAck(ea.DeliveryTag, false);
                    }
                };

                incomingChannel.BasicConsume("aggregations", false, consumer);

                Console.CancelKeyPress += OnExit;
                _closing.WaitOne();
            }
        }

        protected static void OnExit(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("Exit");
            _closing.Set();
        }
    }
}
