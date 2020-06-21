namespace Meteonel.Ingestor.DomainModel
{
    public class ChargeReading : Reading
    {
        public virtual decimal Charge { get; set; }

        public virtual int Temperature { get; set; }
        
        public virtual ChargePower Power { get; set; }
        
        public virtual ChargeStatus Status { get; set; }
    }
}