using Meteonel.DomainModel;
using Meteonel.Ingestor.Messages;
using NHibernate;

namespace Meteonel.Ingestor.Ingestors
{
    public class Ds18B20Ingestor : TemplateIngestor<Ds18B20Message, Ds18B20Reading, Ds18B20Latest>
    {
        public Ds18B20Ingestor(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        protected override string QueueName => "ds18b20_persisted";

        protected override void PopulateReading(Ds18B20Message message, Ds18B20Reading reading)
        {
            Populate(message, reading);
        }

        protected override void PopulateLatest(Ds18B20Message message, Ds18B20Latest latest)
        {
            Populate(message, latest);
        }

        private static void Populate(Ds18B20Message message, IDs18B20Reading destination)
        {
            destination.Timestamp = message.Timestamp;
            destination.TempGround = message.TempGround;
        }
    }
}