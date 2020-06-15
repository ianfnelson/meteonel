using System.Text.Json.Serialization;

namespace Meteonel.Ingestor.Messages
{
    public class RainTipMessage : Message
    {
        [JsonPropertyName("rain")]
        public decimal Rain { get; set; }
    }
}