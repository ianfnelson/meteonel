using System;
using System.Text.Json.Serialization;

namespace Meteonel.Ingestor.Messages
{
    public abstract class Message : IMessage
    {
        [JsonPropertyName("device")]
        public string Device { get; set; }
        
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}