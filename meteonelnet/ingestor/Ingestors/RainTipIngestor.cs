using Meteonel.Ingestor.DomainModel;
using Meteonel.Ingestor.Messages;
using NHibernate;

namespace Meteonel.Ingestor.Ingestors
{
    public class RainTipIngestor : TemplateIngestor<RainTipMessage, RainTipReading>
    {
        public RainTipIngestor(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        protected override string QueueName => "raintip_persisted";
        protected override RainTipReading Map(RainTipMessage message)
        {
            var reading = new RainTipReading
            {
                Device = GetDevice(message.Device),
                TimeStamp = message.Timestamp,
                Rain = message.Rain
            };
            return reading;
        }
    }
}