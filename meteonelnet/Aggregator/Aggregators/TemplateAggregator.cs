using System;
using System.Collections.Generic;
using System.Linq;
using Meteonel.DomainModel;
using NHibernate;

namespace Meteonel.Aggregator.Aggregators
{
    public abstract class TemplateAggregator<TAggregation> : IAggregator
        where TAggregation : Aggregation, new()
    {
        protected readonly ISessionFactory SessionFactory;
        private static IList<Device> _devices;
        private static IList<AggregationPeriod> _aggregationPeriods;

        protected TemplateAggregator(ISessionFactory sessionFactory)
        {
            SessionFactory = sessionFactory;
            GetReferenceData();
        }

        private void GetReferenceData()
        {
            using var session = SessionFactory.OpenStatelessSession();

            _devices = session.Query<Device>().ToList();
            _aggregationPeriods = session.Query<AggregationPeriod>().ToList();
        }

        public abstract SensorType SensorType { get; }

        public void Aggregate(string deviceName)
        {
            foreach (var aggregationPeriod in _aggregationPeriods)
            {
                Aggregate(deviceName, aggregationPeriod);
            }
        }

        private void Aggregate(string deviceName, AggregationPeriod aggregationPeriod)
        {
            using (var session = SessionFactory.OpenSession())
            {
                var device = GetDevice(deviceName);
                var aggregation = GetOrCreateAggregation(session, device, aggregationPeriod);

                if (aggregation.CalculationTimestamp.AddMinutes(aggregationPeriod.RecalculationGraceMinutes) >=
                    DateTime.UtcNow)
                {
                    return;
                }

                if (!ShouldAggregateForPeriod(aggregation))
                {
                    return;
                }

                var minimumDateTime = DateTime.UtcNow.AddDays(-aggregationPeriod.PeriodDays);
                UpdateAggregation(session, aggregation, minimumDateTime);
                aggregation.CalculationTimestamp = DateTime.UtcNow;
                session.SaveOrUpdate(aggregation);
                session.Flush();
            }
        }

        protected virtual bool ShouldAggregateForPeriod(TAggregation aggregation)
        {
            return true;
        }

        protected abstract void UpdateAggregation(ISession session, TAggregation aggregation, DateTime minimumDateTime);

        private static TAggregation GetOrCreateAggregation(ISession session, Device device, AggregationPeriod aggregationPeriod)
        {
            var aggregation = session.Query<TAggregation>()
                .SingleOrDefault(x => x.Device.Id == device.Id && x.Period.Id == aggregationPeriod.Id);
            return aggregation ?? new TAggregation {Device = device, Period = aggregationPeriod};
        }

        private static Device GetDevice(string deviceName)
        {
            return _devices.Single(x => x.Name == deviceName);
        }
    }
}
