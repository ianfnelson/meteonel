using System.Text.Json.Serialization;

namespace Meteonel.Ingestor.Messages
{
    public class Ds18B20Message : Message
    {
        [JsonPropertyName("tempGround")]
        public decimal TempGround { get; set; }
    }
}