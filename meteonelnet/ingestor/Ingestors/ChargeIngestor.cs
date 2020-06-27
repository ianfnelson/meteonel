using System.Collections.Generic;
using System.Linq;
using Meteonel.Ingestor.DomainModel;
using Meteonel.Ingestor.Messages;
using NHibernate;

namespace Meteonel.Ingestor.Ingestors
{
    public class ChargeIngestor : TemplateIngestor<ChargeMessage, ChargeReading>
    {
        private static IList<ChargePower> _powers;
        private static IList<ChargeStatus> _statuses;
        
        public ChargeIngestor(ISessionFactory sessionFactory) : base(sessionFactory)
        {
            GetReferenceData();
        }

        protected override string QueueName => "charge_persisted";

        protected override ChargeReading Map(ChargeMessage message)
        {
            var reading = new ChargeReading
            {
                Device = GetDevice(message.Device),
                Power = GetPower(message.Power),
                Status = GetStatus(message.Status),
                Temperature = message.Temperature,
                TimeStamp = message.Timestamp,
                Charge = message.Charge
            };
            return reading;
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