using System;

namespace Meteonel.DomainModel
{
    public interface IAggregation : IEntity
    {
        Device Device { get; set; }

        AggregationPeriod Period { get; set; }

        DateTime CalculationDateTime { get; set; }
    }
}