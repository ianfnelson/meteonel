using Meteonel.DomainModel;
using Meteonel.Ingestor.Messages;
using RabbitMQ.Client;

namespace Meteonel.Ingestor.Ingestors
{
    public interface IIngestor<TMessage, TReading, TLatest> : IIngestor
        where TMessage : IMessage
        where TReading : IReading
        where TLatest : IReading
    {
    }

    public interface IIngestor
    {
        SensorType SensorType { get; }
        void Ingest(IConnection connection);
    }
}