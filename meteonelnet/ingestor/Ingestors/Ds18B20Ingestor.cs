using Meteonel.Ingestor.DomainModel;
using Meteonel.Ingestor.Messages;
using NHibernate;

namespace Meteonel.Ingestor.Ingestors
{
    public class Ds18B20Ingestor : TemplateIngestor<Ds18B20Message, Ds18B20Reading>
    {
        public Ds18B20Ingestor(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        protected override string QueueName => "ds18b20_persisted";
        protected override Ds18B20Reading Map(Ds18B20Message message)
        {
            var reading = new Ds18B20Reading
            {
                Device = GetDevice(message.Device),
                TimeStamp = message.Timestamp,
                TempGround = message.TempGround
            };
            return reading;
        }
    }
}