using System;
using System.Linq;
using Meteonel.DomainModel;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace Meteonel.Aggregator.Aggregators
{
    public class Bme280Aggregator : TemplateAggregator<Bme280Aggregation>
    {
        public Bme280Aggregator(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        public override SensorType SensorType => SensorType.Bme280;

        protected override void UpdateAggregation(ISession session, Bme280Aggregation aggregation, DateTime minimumDateTime)
        {
            UpdateTemp(session, aggregation, minimumDateTime);
            UpdateHumidity(session, aggregation, minimumDateTime);
            UpdatePressure(session, aggregation, minimumDateTime);
        }

        private static void UpdatePressure(ISession session, Bme280Aggregation aggregation, DateTime minimumDateTime)
        {
            var futureMinimumPressure = session.Query<Bme280Reading>()
                .Where(x => x.Timestamp >= minimumDateTime)
                .Where(x => x.Device.Id == aggregation.Device.Id)
                .OrderBy(x => x.Pressure)
                .ThenBy(x => x.Timestamp)
                .Take(1)
                .ToFuture();

            var futureMaximumPressure = session.Query<Bme280Reading>()
                .Where(x => x.Timestamp >= minimumDateTime)
                .Where(x => x.Device.Id == aggregation.Device.Id)
                .OrderByDescending(x => x.Pressure)
                .ThenBy(x => x.Timestamp)
                .Take(1)
                .ToFuture();

            var futureAveragePressure = session.CreateCriteria<Bme280Reading>()
                .Add(Restrictions.Eq("Device.Id", aggregation.Device.Id))
                .Add(Restrictions.Ge("Timestamp", minimumDateTime))
                .SetProjection(Projections.Avg("Pressure"))
                .FutureValue<double>();

            var minimumPressure = futureMinimumPressure.ToList().Single();
            var maximumPressure = futureMaximumPressure.ToList().Single();

            aggregation.PressureAverage = Convert.ToDecimal(futureAveragePressure.Value);

            aggregation.PressureMinimum = minimumPressure.Pressure;
            aggregation.PressureMinimumTimestamp = minimumPressure.Timestamp;

            aggregation.PressureMaximum = maximumPressure.Pressure;
            aggregation.PressureMaximumTimestamp = maximumPressure.Timestamp;
        }

        private static void UpdateHumidity(ISession session, Bme280Aggregation aggregation, DateTime minimumDateTime)
        {
            var futureMinimumHumidity = session.Query<Bme280Reading>()
                .Where(x => x.Timestamp >= minimumDateTime)
                .Where(x => x.Device.Id == aggregation.Device.Id)
                .OrderBy(x => x.Humidity)
                .ThenBy(x => x.Timestamp)
                .Take(1)
                .ToFuture();

            var futureMaximumHumidity = session.Query<Bme280Reading>()
                .Where(x => x.Timestamp >= minimumDateTime)
                .Where(x => x.Device.Id == aggregation.Device.Id)
                .OrderByDescending(x => x.Humidity)
                .ThenBy(x => x.Timestamp)
                .Take(1)
                .ToFuture();

            var futureAverageHumidity = session.CreateCriteria<Bme280Reading>()
                .Add(Restrictions.Eq("Device.Id", aggregation.Device.Id))
                .Add(Restrictions.Ge("Timestamp", minimumDateTime))
                .SetProjection(Projections.Avg("Humidity"))
                .FutureValue<double>();

            var minimumHumidity = futureMinimumHumidity.ToList().Single();
            var maximumHumidity = futureMaximumHumidity.ToList().Single();

            aggregation.HumidityAverage = Convert.ToDecimal(futureAverageHumidity.Value);

            aggregation.HumidityMinimum = minimumHumidity.Humidity;
            aggregation.HumidityMinimumTimestamp = minimumHumidity.Timestamp;

            aggregation.HumidityMaximum = maximumHumidity.Humidity;
            aggregation.HumidityMaximumTimestamp = maximumHumidity.Timestamp;
        }

        private static void UpdateTemp(ISession session, Bme280Aggregation aggregation, DateTime minimumDateTime)
        {
            var futureMinimumTemp = session.Query<Bme280Reading>()
                .Where(x => x.Timestamp >= minimumDateTime)
                .Where(x => x.Device.Id == aggregation.Device.Id)
                .OrderBy(x => x.TempAmbient)
                .ThenBy(x => x.Timestamp)
                .Take(1)
                .ToFuture();

            var futureMaximumTemp = session.Query<Bme280Reading>()
                .Where(x => x.Timestamp >= minimumDateTime)
                .Where(x => x.Device.Id == aggregation.Device.Id)
                .OrderByDescending(x => x.TempAmbient)
                .ThenBy(x => x.Timestamp)
                .Take(1)
                .ToFuture();

            var futureAverageTemp = session.CreateCriteria<Bme280Reading>()
                .Add(Restrictions.Eq("Device.Id", aggregation.Device.Id))
                .Add(Restrictions.Ge("Timestamp", minimumDateTime))
                .SetProjection(Projections.Avg("TempAmbient"))
                .FutureValue<double>();

            var minimumTemp = futureMinimumTemp.ToList().Single();
            var maximumTemp = futureMaximumTemp.ToList().Single();

            aggregation.TempAmbientAverage = Convert.ToDecimal(futureAverageTemp.Value);

            aggregation.TempAmbientMinimum = minimumTemp.TempAmbient;
            aggregation.TempAmbientMinimumTimestamp = minimumTemp.Timestamp;

            aggregation.TempAmbientMaximum = maximumTemp.TempAmbient;
            aggregation.TempAmbientMaximumTimestamp = maximumTemp.Timestamp;
        }
    }
}
