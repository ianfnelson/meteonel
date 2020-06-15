using Meteonel.Ingestor.DomainModel;
using Meteonel.Ingestor.Messages;
using NHibernate;

namespace Meteonel.Ingestor.Ingestors
{
    public class WindIngestor : TemplateIngestor<WindMessage, WindReading>
    {
        public WindIngestor(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        protected override string QueueName => "wind_persisted";
        protected override WindReading Map(WindMessage message)
        {
            var reading = new WindReading
            {
                Device = GetDevice(message.Device),
                TimeStamp = message.Timestamp,
                WindDirection = message.WindDirection,
                WindGust = message.WindGust,
                WindSpeed = message.WindSpeed
            };
            return reading;
        }
    }
}