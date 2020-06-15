using System;
using System.Threading;
using Meteonel.Ingestor.Ingestors;
using NHibernate;
using RabbitMQ.Client;

namespace Meteonel.Ingestor
{
    class Program
    {
        private static readonly ISessionFactory _sessionFactory = new FactoryManager().Instance;
        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);
        
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                HostName="192.168.1.97",
                UserName="meteonel",
                Password = "7Z*0f4QRHOuO",
                VirtualHost = "meteonel"
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var bme280Ingestor = new Bme280Ingestor(_sessionFactory);
                    bme280Ingestor.Ingest(channel);
                    
                    var ds18b20Ingestor = new Ds18B20Ingestor(_sessionFactory);
                    ds18b20Ingestor.Ingest(channel);
                    
                    var chargeIngestor = new ChargeIngestor(_sessionFactory);
                    chargeIngestor.Ingest(channel);
                    
                    var rainTipIngestor = new RainTipIngestor(_sessionFactory);
                    rainTipIngestor.Ingest(channel);
                    
                    var windIngestor = new WindIngestor(_sessionFactory);
                    windIngestor.Ingest(channel);

                    Console.CancelKeyPress += OnExit;
                    _closing.WaitOne();
                }
            }
        }

        protected static void OnExit(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("Exit");
            _closing.Set();
        }
    }
}