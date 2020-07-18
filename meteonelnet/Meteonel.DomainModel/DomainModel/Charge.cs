using System;

namespace Meteonel.DomainModel
{
    public class ChargeLatest : Reading, IChargeReading
    {
        public virtual int Charge { get; set; }
        public virtual int Temperature { get; set; }
        public virtual ChargePower Power { get; set; }
        public virtual ChargeStatus Status { get; set; }
    }

    public class ChargeReading : Reading, IChargeReading
    {
        public virtual int Charge { get; set; }
        public virtual int Temperature { get; set; }
        public virtual ChargePower Power { get; set; }
        public virtual ChargeStatus Status { get; set; }
    }

    public class ChargeAggregation : Aggregation
    {
        public virtual int ChargeMaximum { get; set; }
        public virtual DateTime ChargeMaximumTimestamp { get; set; }
        public virtual int ChargeMinimum { get; set; }
        public virtual DateTime ChargeMinimumTimestamp { get; set; }
        public virtual decimal ChargeAverage { get; set; }
    }

    public class ChargePower : ReferenceData
    {
    }

    public class ChargeStatus : ReferenceData
    {
    }

    public interface IChargeReading : IReading
    {
        int Charge { get; set; }
        int Temperature { get; set; }
        ChargePower Power { get; set; }
        ChargeStatus Status { get; set; }
    }
}