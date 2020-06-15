using Meteonel.Ingestor.DomainModel;
using Meteonel.Ingestor.Messages;
using NHibernate;

namespace Meteonel.Ingestor.Ingestors
{
    public class ChargeIngestor : TemplateIngestor<ChargeMessage, ChargeReading>
    {
        public ChargeIngestor(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        protected override string QueueName => "charge_persisted";
        protected override ChargeReading Map(ChargeMessage message)
        {
            var reading = new ChargeReading
            {
                Device = GetDevice(message.Device),
                TimeStamp = message.Timestamp,
                Charge = message.Charge
            };
            return reading;
        }
    }
}