namespace Meteonel.DomainModel
{
    public class ChargeLatest : Reading, IChargeReading
    {
        public virtual decimal Charge { get; set; }
        public virtual int Temperature { get; set; }
        public virtual ChargePower Power { get; set; }
        public virtual ChargeStatus Status { get; set; }
    }

    public class ChargeReading : Reading, IChargeReading
    {
        public virtual decimal Charge { get; set; }
        public virtual int Temperature { get; set; }
        public virtual ChargePower Power { get; set; }
        public virtual ChargeStatus Status { get; set; }
    }

    public class ChargePower : ReferenceData
    {
    }

    public class ChargeStatus : ReferenceData
    {
    }

    public interface IChargeReading : IReading
    {
        decimal Charge { get; set; }
        int Temperature { get; set; }
        ChargePower Power { get; set; }
        ChargeStatus Status { get; set; }
    }
}