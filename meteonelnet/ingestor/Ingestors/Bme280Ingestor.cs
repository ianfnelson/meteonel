using Meteonel.DomainModel;
using Meteonel.Ingestor.Messages;
using NHibernate;

namespace Meteonel.Ingestor.Ingestors
{
    public class Bme280Ingestor : TemplateIngestor<Bme280Message, Bme280Reading, Bme280Latest>
    {
        public Bme280Ingestor(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        protected override string QueueName => "bme280_persisted";
        public override SensorType SensorType => SensorType.Bme280;

        protected override void PopulateLatest(Bme280Message message, Bme280Latest latest)
        {
            Populate(message, latest);
        }

        protected override void PopulateReading(Bme280Message message, Bme280Reading reading)
        {
            Populate(message, reading);
        }

        private static void Populate(Bme280Message message, IBme280Reading destination)
        {
            destination.Humidity = message.Humidity;
            destination.Pressure = message.Pressure;
            destination.TempAmbient = message.TempAmbient;
        }
    }
}