using Meteonel.DomainModel;
using Meteonel.Ingestor.Messages;
using NHibernate;

namespace Meteonel.Ingestor.Ingestors
{
    public class WindIngestor : TemplateIngestor<WindMessage, WindReading, WindLatest>
    {
        public WindIngestor(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        protected override string QueueName => "wind";
        public override SensorType SensorType => SensorType.Wind;

        protected override void PopulateReading(WindMessage message, WindReading reading)
        {
            Populate(message, reading);
        }

        protected override void PopulateLatest(WindMessage message, WindLatest latest)
        {
            Populate(message, latest);
        }

        private static void Populate(WindMessage message, IWindReading destination)
        {
            destination.Timestamp = message.Timestamp;
            destination.WindDirection = message.WindDirection;
            destination.WindGust = message.WindGust;
            destination.WindSpeed = message.WindSpeed;
        }
    }
}