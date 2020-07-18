namespace Meteonel.DomainModel
{
    public class Ds18B20Latest : Reading, IDs18B20Reading
    {
        public virtual decimal TempGround { get; set; }
    }

    public class Ds18B20Reading : Reading, IDs18B20Reading
    {
        public virtual decimal TempGround { get; set; }
    }

    public interface IDs18B20Reading : IReading
    {
        decimal TempGround { get; set; }
    }
}