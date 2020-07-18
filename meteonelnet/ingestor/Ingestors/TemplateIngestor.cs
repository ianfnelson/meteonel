using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Meteonel.DomainModel;
using Meteonel.Ingestor.Messages;
using NHibernate;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Meteonel.Ingestor.Ingestors
{
    public abstract class TemplateIngestor<TMessage, TReading, TLatest> : IIngestor<TMessage, TReading, TLatest>
        where TMessage : IMessage
        where TReading : IReading, new()
        where TLatest : IReading, new()
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

                using (var session = SessionFactory.OpenSession())
                {
                    try
                    {
                        var device = GetDevice(message);

                        var reading = new TReading();
                        reading.Device = device;
                        reading.Timestamp = message.Timestamp;
                        PopulateReading(message, reading);

                        var latest = GetOrCreateLatest(session, device);
                        latest.Device = device;
                        latest.Timestamp = message.Timestamp;
                        PopulateLatest(message, latest);

                        session.SaveOrUpdate(latest);
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

        private static TLatest GetOrCreateLatest(ISession session, Device device)
        {
            var latest = session.Query<TLatest>().SingleOrDefault(x => x.Device.Id == device.Id);
            return latest ?? new TLatest();
        }

        private static Device GetDevice(TMessage message)
        {
            return _devices.Single(x => x.Name == message.Device);
        }

        private IList<Device> GetDevices()
        {
            using var session = SessionFactory.OpenStatelessSession();
            
            var devices = session.Query<Device>().ToList();
            return devices;
        }

        protected abstract void PopulateReading(TMessage message, TReading reading);
        protected abstract void PopulateLatest(TMessage message, TLatest latest);
    }
}