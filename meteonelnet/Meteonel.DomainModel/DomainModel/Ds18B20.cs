using System;

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

    public class Ds18B20Aggregation : Aggregation
    {
        public virtual decimal TempGroundMaximum { get; set; }
        public virtual DateTime TempGroundMaximumTimestamp { get; set; }
        public virtual decimal TempGroundMinimum { get; set; }
        public virtual DateTime TempGroundMinimumTimestamp { get; set; }
        public virtual decimal TempGroundAverage { get; set; }
    }

    public interface IDs18B20Reading : IReading
    {
        decimal TempGround { get; set; }
    }
}