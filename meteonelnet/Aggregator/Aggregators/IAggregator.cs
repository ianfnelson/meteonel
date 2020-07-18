namespace Meteonel.Aggregator.Aggregators
{
    public interface IAggregator
    {
        SensorType SensorType { get; }
        void Aggregate(string deviceName);
    }
}