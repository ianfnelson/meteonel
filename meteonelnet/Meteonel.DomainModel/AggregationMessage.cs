using System.Text.Json.Serialization;

namespace Meteonel
{
    public class AggregationMessage
    {
        [JsonPropertyName("sensorType")]
        public SensorType SensorType { get; set; }

        [JsonPropertyName("device")]
        public string Device { get; set; }
    }
}
