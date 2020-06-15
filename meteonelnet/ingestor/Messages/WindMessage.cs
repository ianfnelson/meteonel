using System.Text.Json.Serialization;

namespace Meteonel.Ingestor.Messages
{
    public class WindMessage : Message
    {
        [JsonPropertyName("windSpeed")]
        public decimal WindSpeed { get; set; }
        
        [JsonPropertyName("windGust")]
        public decimal WindGust { get; set; }
        
        [JsonPropertyName("windDirection")]
        public decimal WindDirection { get; set; }
    }
}