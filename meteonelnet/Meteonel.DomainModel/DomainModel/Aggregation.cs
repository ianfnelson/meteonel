using System;

namespace Meteonel.DomainModel
{
    public abstract class Aggregation : IAggregation
    {
        public virtual int Id { get; set; }
        public virtual Device Device { get; set; }
        public virtual AggregationPeriod Period { get; set; }
        public virtual DateTime CalculationDateTime { get; set; }
    }
}