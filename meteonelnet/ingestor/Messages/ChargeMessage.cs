using System.Text.Json.Serialization;

namespace Meteonel.Ingestor.Messages
{
    public class ChargeMessage : Message
    {
        [JsonPropertyName("charge")]
        public decimal Charge { get; set; }
        
        [JsonPropertyName("temperature")]
        public int Temperature { get; set; }
        
        [JsonPropertyName("status")]
        public string Status { get; set; }
        
        [JsonPropertyName("power")]
        public string Power { get; set; }
    }
}