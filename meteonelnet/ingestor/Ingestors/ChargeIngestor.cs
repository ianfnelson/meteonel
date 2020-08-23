using System.Collections.Generic;
using System.Linq;
using Meteonel.DomainModel;
using Meteonel.Ingestor.Messages;
using NHibernate;

namespace Meteonel.Ingestor.Ingestors
{
    public class ChargeIngestor : TemplateIngestor<ChargeMessage, ChargeReading, ChargeLatest>
    {
        private static IList<ChargePower> _powers;
        private static IList<ChargeStatus> _statuses;
        
        public ChargeIngestor(ISessionFactory sessionFactory) : base(sessionFactory)
        {
            GetReferenceData();
        }

        protected override string QueueName => "charge";
        public override SensorType SensorType => SensorType.Charge;

        protected override void PopulateReading(ChargeMessage message, ChargeReading reading)
        {
            Populate(message, reading);
        }

        protected override void PopulateLatest(ChargeMessage message, ChargeLatest latest)
        {
            Populate(message, latest);
        }

        private static void Populate(ChargeMessage message, IChargeReading destination)
        {
            destination.Power = GetPower(message.Power);
            destination.Status = GetStatus(message.Status);
            destination.Temperature = message.Temperature;
            destination.Timestamp = message.Timestamp;
            destination.Charge = message.Charge;
        }

        private static ChargeStatus GetStatus(string statusCode)
        {
            return _statuses.Single(x => x.Code == statusCode);
        }

        private static ChargePower GetPower(string powerCode)
        {
            return _powers.Single(x => x.Code == powerCode);
        }

        private void GetReferenceData()
        {
            using var session = SessionFactory.OpenStatelessSession();

            _powers = session.Query<ChargePower>().ToList();
            _statuses = session.Query<ChargeStatus>().ToList();
        }
    }
}