using Meteonel.Ingestor.DomainModel;
using Meteonel.Ingestor.Messages;
using RabbitMQ.Client;

namespace Meteonel.Ingestor.Ingestors
{
    public interface IIngestor<TMessage, TReading> : IIngestor
        where TMessage : IMessage
        where TReading : IReading
    {
    }

    public interface IIngestor
    {
        void Ingest(IConnection connection);
    }
}