namespace Meteonel.DomainModel
{
    public class WindLatest : Reading, IWindReading
    {
        public virtual decimal WindSpeed { get; set; }
        public virtual decimal WindGust { get; set; }
        public virtual decimal WindDirection { get; set; }
    }

    public class WindReading : Reading, IWindReading
    {
        public virtual decimal WindSpeed { get; set; }
        public virtual decimal WindGust { get; set; }
        public virtual decimal WindDirection { get; set; }
    }

    public interface IWindReading : IReading
    {
        decimal WindSpeed { get; set; }
        decimal WindGust { get; set; }
        decimal WindDirection { get; set; }
    }
}
