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

    public class Bme280Aggregation : Aggregation
    {
        public virtual decimal TempAmbientMaximum { get; set; }
        public virtual decimal TempAmbientMinimum { get; set; }
        public virtual decimal TempAmbientAverage { get; set; }

        public virtual decimal HumidityMaximum { get; set; }
        public virtual decimal HumidityMinimum { get; set; }
        public virtual decimal HumidityAverage { get; set; }

        public virtual decimal PressureMaximum { get; set; }
        public virtual decimal PressureMinimum { get; set; }
        public virtual decimal PressureAverage { get; set; }
    }

    public interface IBme280Reading : IReading
    {
        decimal TempAmbient { get; set; }
        decimal Humidity { get; set; }
        decimal Pressure { get; set; }
    }
}