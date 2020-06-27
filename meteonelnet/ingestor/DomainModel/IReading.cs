using System;

namespace Meteonel.Ingestor.DomainModel
{
    public interface IReading : IEntity
    {
        Device Device { get; set; }
        
        DateTime TimeStamp { get; set; }
    }
}