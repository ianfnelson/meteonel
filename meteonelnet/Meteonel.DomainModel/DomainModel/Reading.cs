using System;

namespace Meteonel.DomainModel
{
    public abstract class Reading : IReading
    {
        public virtual int Id { get; set; }
        public virtual Device Device { get; set; }
        public virtual DateTime Timestamp { get; set; }
    }
}