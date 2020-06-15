namespace Meteonel.Ingestor.DomainModel
{
    public class WindReading : Reading
    {
        public virtual decimal WindSpeed { get; set; }
        
        public virtual decimal WindGust { get; set; }
        
        public virtual decimal WindDirection { get; set; }
    }
}