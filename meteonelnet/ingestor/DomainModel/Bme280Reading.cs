namespace Meteonel.Ingestor.DomainModel
{
    public class Bme280Reading : Reading
    {
        public virtual decimal TempAmbient { get; set; }
        
        public virtual decimal Humidity { get; set; }
        
        public virtual decimal Pressure { get; set; }
    }
}