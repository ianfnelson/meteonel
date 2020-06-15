namespace Meteonel.Ingestor.DomainModel
{
    public class Ds18B20Reading : Reading
    {
        public virtual decimal TempGround { get; set; }
    }
}