using System;
using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace Meteonel.Ingestor
{
    public class Bme280Message
    {
        [J("device")]
        public string Device { get; set; }
        
        [J("timestamp")]
        public DateTime Timestamp { get; set; }
        
        [J("tempAmbient")]
        public decimal TempAmbient { get; set; }
        
        [J("humidity")]
        public decimal Humidity { get; set; }
        
        [J("pressure")]
        public decimal Pressure { get; set; }
    }
}