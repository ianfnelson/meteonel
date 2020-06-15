using System;

namespace Meteonel.Ingestor.DomainModel
{
    public interface IReading
    {
        int Id { get; set; }
        
        Device Device { get; set; }
        
        DateTime TimeStamp { get; set; }
    }
}