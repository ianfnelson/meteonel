using Meteonel.Ingestor.DomainModel;
using Meteonel.Ingestor.Messages;
using NHibernate;

namespace Meteonel.Ingestor.Ingestors
{
    public class Bme280Ingestor : TemplateIngestor<Bme280Message, Bme280Reading>
    {
        public Bme280Ingestor(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        protected override string QueueName => "bme280_persisted";
        
        protected override Bme280Reading Map(Bme280Message message)
        {
            var reading = new Bme280Reading
            {
                Device = GetDevice(message.Device),
                TimeStamp = message.Timestamp,
                Humidity = message.Humidity,
                Pressure = message.Pressure,
                TempAmbient = message.TempAmbient
            };
            return reading;
        }
    }
}