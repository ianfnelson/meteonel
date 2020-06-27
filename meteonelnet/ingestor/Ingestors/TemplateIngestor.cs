using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Meteonel.Ingestor.DomainModel;
using Meteonel.Ingestor.Messages;
using NHibernate;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Meteonel.Ingestor.Ingestors
{
    public abstract class TemplateIngestor<TMessage, TReading> : IIngestor<TMessage, TReading>
        where TMessage : IMessage
        where TReading : IReading
    {
        protected readonly ISessionFactory SessionFactory;
        private static IList<Device> _devices;

        protected TemplateIngestor(ISessionFactory sessionFactory)
        {
            SessionFactory = sessionFactory;
            _devices = GetDevices();
        }

        protected abstract string QueueName { get; }
        
        public void Ingest(IConnection connection)
        {
            var channel = connection.CreateModel();
                
            channel.QueueDeclare(QueueName,
                true,
                false,
                false,
                new Dictionary<string, object> {{"x-queue-type", "quorum"}});
            
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var bodyBytes = ea.Body;
                var bodyString = Encoding.UTF8.GetString(bodyBytes);
                var message = JsonSerializer.Deserialize<TMessage>(bodyString);

                var reading = Map(message);

                using (var session = SessionFactory.OpenSession())
                {
                    try
                    {
                        session.Save(reading);
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

            channel.BasicConsume(QueueName, false, consumer);
        }

        protected abstract TReading Map(TMessage message);

        protected Device GetDevice(string deviceName)
        {
            return _devices.Single(x => x.Name == deviceName);
        }

        private IList<Device> GetDevices()
        {
            using var session = SessionFactory.OpenStatelessSession();
            
            var devices = session.Query<Device>().ToList();
            return devices;
        }
    }
}