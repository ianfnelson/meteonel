using System;

namespace Meteonel.Ingestor.Messages
{
    public interface IMessage
    {
        string Device { get; set; }
        
        DateTime Timestamp { get; set; }
    }
}