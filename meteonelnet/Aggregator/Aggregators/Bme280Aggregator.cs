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

        protected override void UpdateAggregation(ISession session, Bme280Aggregation aggregation, DateTime minimumDate)
        {
            var futureMinimumTemp = session.Query<Bme280Reading>()
                .Where(x => x.Timestamp >= minimumDate)
                .Where(x => x.Device.Id == aggregation.Device.Id)
                .OrderBy(x => x.TempAmbient)
                .ThenBy(x => x.Timestamp)
                .Take(1)
                .ToFuture();

            var futureMaximumTemp = session.Query<Bme280Reading>()
                .Where(x => x.Timestamp >= minimumDate)
                .Where(x => x.Device.Id == aggregation.Device.Id)
                .OrderByDescending(x => x.TempAmbient)
                .ThenBy(x => x.Timestamp)
                .Take(1)
                .ToFuture();

            var futureAverageTemp = session.CreateCriteria<Bme280Reading>()
                .Add(Restrictions.Eq("Device.Id", aggregation.Device.Id))
                .Add(Restrictions.Ge("Timestamp", minimumDate))
                .SetProjection(Projections.Avg("TempAmbient"))
                .FutureValue<double>();

            var futureMinimumHumidity = session.Query<Bme280Reading>()
                .Where(x => x.Timestamp >= minimumDate)
                .Where(x => x.Device.Id == aggregation.Device.Id)
                .OrderBy(x => x.Humidity)
                .ThenBy(x => x.Timestamp)
                .Take(1)
                .ToFuture();

            var futureMaximumHumidity = session.Query<Bme280Reading>()
                .Where(x => x.Timestamp >= minimumDate)
                .Where(x => x.Device.Id == aggregation.Device.Id)
                .OrderByDescending(x => x.Humidity)
                .ThenBy(x => x.Timestamp)
                .Take(1)
                .ToFuture();

            var futureAverageHumidity = session.CreateCriteria<Bme280Reading>()
                .Add(Restrictions.Eq("Device.Id", aggregation.Device.Id))
                .Add(Restrictions.Ge("Timestamp", minimumDate))
                .SetProjection(Projections.Avg("Humidity"))
                .FutureValue<double>();

            var futureMinimumPressure = session.Query<Bme280Reading>()
                .Where(x => x.Timestamp >= minimumDate)
                .Where(x => x.Device.Id == aggregation.Device.Id)
                .OrderBy(x => x.Pressure)
                .ThenBy(x => x.Timestamp)
                .Take(1)
                .ToFuture();

            var futureMaximumPressure = session.Query<Bme280Reading>()
                .Where(x => x.Timestamp >= minimumDate)
                .Where(x => x.Device.Id == aggregation.Device.Id)
                .OrderByDescending(x => x.Pressure)
                .ThenBy(x => x.Timestamp)
                .Take(1)
                .ToFuture();

            var futureAveragePressure = session.CreateCriteria<Bme280Reading>()
                .Add(Restrictions.Eq("Device.Id", aggregation.Device.Id))
                .Add(Restrictions.Ge("Timestamp", minimumDate))
                .SetProjection(Projections.Avg("Pressure"))
                .FutureValue<double>();

            var minimumTemp = futureMinimumTemp.ToList().Single();
            var maximumTemp = futureMaximumTemp.ToList().Single();
            var minimumHumidity = futureMinimumHumidity.ToList().Single();
            var maximumHumidity = futureMaximumHumidity.ToList().Single();
            var minimumPressure = futureMinimumPressure.ToList().Single();
            var maximumPressure = futureMaximumPressure.ToList().Single();

            aggregation.TempAmbientAverage = Convert.ToDecimal(futureAverageTemp.Value);
            aggregation.HumidityAverage = Convert.ToDecimal(futureAverageHumidity.Value);
            aggregation.PressureAverage = Convert.ToDecimal(futureAveragePressure.Value);

            aggregation.TempAmbientMinimum = minimumTemp.TempAmbient;
            aggregation.TempAmbientMinimumTimestamp = minimumTemp.Timestamp;

            aggregation.TempAmbientMaximum = maximumTemp.TempAmbient;
            aggregation.TempAmbientMaximumTimestamp = maximumTemp.Timestamp;

            aggregation.HumidityMinimum = minimumHumidity.Humidity;
            aggregation.HumidityMinimumTimestamp = minimumHumidity.Timestamp;

            aggregation.HumidityMaximum = maximumHumidity.Humidity;
            aggregation.HumidityMaximumTimestamp = maximumHumidity.Timestamp;

            aggregation.PressureMinimum = minimumPressure.Pressure;
            aggregation.PressureMinimumTimestamp = minimumPressure.Timestamp;

            aggregation.PressureMaximum = maximumPressure.Pressure;
            aggregation.PressureMaximumTimestamp = maximumPressure.Timestamp;
        }
    }
}
