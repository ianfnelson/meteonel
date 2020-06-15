using System;

namespace Meteonel.Ingestor.DomainModel
{
    public abstract class Reading : IReading
    {
        public virtual int Id { get; set; }
        
        public virtual Device Device { get; set; }
        
        public virtual DateTime TimeStamp { get; set; }
    }
}