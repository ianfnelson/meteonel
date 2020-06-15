using System.Text.Json.Serialization;

namespace Meteonel.Ingestor.Messages
{
    public class ChargeMessage : Message
    {
        [JsonPropertyName("charge")]
        public decimal Charge { get; set; }
    }
}