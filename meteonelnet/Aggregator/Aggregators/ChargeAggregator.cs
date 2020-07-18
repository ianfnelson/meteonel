using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meteonel.DomainModel;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace Meteonel.Aggregator.Aggregators
{
    public class ChargeAggregator : TemplateAggregator<ChargeAggregation>
    {
        public ChargeAggregator(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        public override SensorType SensorType => SensorType.Charge;

        protected override void UpdateAggregation(ISession session, ChargeAggregation aggregation, DateTime minimumDateTime)
        {
            var futureMinimumCharge = session.Query<ChargeReading>()
                .Where(x => x.Timestamp >= minimumDateTime)
                .Where(x => x.Device.Id == aggregation.Device.Id)
                .OrderBy(x => x.Charge)
                .ThenBy(x => x.Timestamp)
                .Take(1)
                .ToFuture();

            var futureMaximumCharge = session.Query<ChargeReading>()
                .Where(x => x.Timestamp >= minimumDateTime)
                .Where(x => x.Device.Id == aggregation.Device.Id)
                .OrderByDescending(x => x.Charge)
                .ThenBy(x => x.Timestamp)
                .Take(1)
                .ToFuture();

            var futureAverageCharge = session.CreateCriteria<ChargeReading>()
                .Add(Restrictions.Eq("Device.Id", aggregation.Device.Id))
                .Add(Restrictions.Ge("Timestamp", minimumDateTime))
                .SetProjection(Projections.Avg("Charge"))
                .FutureValue<double>();

            var minimumCharge = futureMinimumCharge.ToList().Single();
            var maximumCharge = futureMaximumCharge.ToList().Single();

            aggregation.ChargeAverage = Convert.ToDecimal(futureAverageCharge.Value);

            aggregation.ChargeMinimum = minimumCharge.Charge;
            aggregation.ChargeMinimumTimestamp = minimumCharge.Timestamp;

            aggregation.ChargeMaximum = maximumCharge.Charge;
            aggregation.ChargeMaximumTimestamp = maximumCharge.Timestamp;
        }

        protected override bool ShouldAggregateForPeriod(ChargeAggregation aggregation)
        {
            return new[] {"24H", "7D"}.Contains(aggregation.Period.Code);
        }
    }
}
