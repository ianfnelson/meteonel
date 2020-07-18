namespace Meteonel.DomainModel
{
    public class Bme280Latest : Reading, IBme280Reading
    {
        public virtual decimal TempAmbient { get; set; }
        public virtual decimal Humidity { get; set; }
        public virtual decimal Pressure { get; set; }
    }

    public class Bme280Reading : Reading, IBme280Reading
    {
        public virtual decimal TempAmbient { get; set; }
        public virtual decimal Humidity { get; set; }
        public virtual decimal Pressure { get; set; }
    }

    public interface IBme280Reading : IReading
    {
        decimal TempAmbient { get; set; }
        decimal Humidity { get; set; }
        decimal Pressure { get; set; }
    }
}