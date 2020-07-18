using System;
using System.Linq;
using Meteonel.DomainModel;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace Meteonel.Aggregator.Aggregators
{
    public class Ds18B20Aggregator : TemplateAggregator<Ds18B20Aggregation>
    {
        public Ds18B20Aggregator(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        public override SensorType SensorType => SensorType.Ds18B20;

        protected override void UpdateAggregation(ISession session, Ds18B20Aggregation aggregation, DateTime minimumDateTime)
        {
            var futureMinimumTemp = session.Query<Ds18B20Reading>()
                .Where(x => x.Timestamp >= minimumDateTime)
                .Where(x => x.Device.Id == aggregation.Device.Id)
                .OrderBy(x => x.TempGround)
                .ThenBy(x => x.Timestamp)
                .Take(1)
                .ToFuture();

            var futureMaximumTemp = session.Query<Ds18B20Reading>()
                .Where(x => x.Timestamp >= minimumDateTime)
                .Where(x => x.Device.Id == aggregation.Device.Id)
                .OrderByDescending(x => x.TempGround)
                .ThenBy(x => x.Timestamp)
                .Take(1)
                .ToFuture();

            var futureAverageTemp = session.CreateCriteria<Ds18B20Reading>()
                .Add(Restrictions.Eq("Device.Id", aggregation.Device.Id))
                .Add(Restrictions.Ge("Timestamp", minimumDateTime))
                .SetProjection(Projections.Avg("TempGround"))
                .FutureValue<double>();
        }
    }
}
