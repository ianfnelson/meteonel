using J = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace Meteonel.Ingestor.Messages
{
    public class Bme280Message : Message
    {
        [J("tempAmbient")]
        public decimal TempAmbient { get; set; }
        
        [J("humidity")]
        public decimal Humidity { get; set; }
        
        [J("pressure")]
        public decimal Pressure { get; set; }
    }
}