using Meteonel.DomainModel;
using Meteonel.Ingestor.Messages;
using NHibernate;

namespace Meteonel.Ingestor.Ingestors
{
    public class RainTipIngestor : TemplateIngestor<RainTipMessage, RainTipReading, RainTipLatest>
    {
        public RainTipIngestor(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        protected override string QueueName => "raintip";
        public override SensorType SensorType => SensorType.Rain;

        protected override void PopulateReading(RainTipMessage message, RainTipReading reading)
        {
            Populate(message, reading);
        }

        protected override void PopulateLatest(RainTipMessage message, RainTipLatest latest)
        {
            Populate(message, latest);
        }

        private static void Populate(RainTipMessage message, IRainTipReading destination)
        {
            destination.Timestamp = message.Timestamp;
            destination.Rain = message.Rain;
        }
    }
}