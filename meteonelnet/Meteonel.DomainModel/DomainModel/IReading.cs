using System;

namespace Meteonel.DomainModel
{
    public interface IReading : IEntity
    {
        Device Device { get; set; }
        
        DateTime Timestamp { get; set; }
    }
}