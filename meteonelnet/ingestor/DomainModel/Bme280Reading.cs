using System;

namespace Meteonel.Ingestor.DomainModel
{
    public class Bme280Reading
    {
        public virtual int Id { get; set; }
        
        public virtual Device Device { get; set; }
        
        public virtual DateTime TimeStamp { get; set; }
        
        public virtual decimal TempAmbient { get; set; }
        
        public virtual decimal Humidity { get; set; }
        
        public virtual decimal Pressure { get; set; }
    }
}