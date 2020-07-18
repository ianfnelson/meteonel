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
            //HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();

            var factory = new ConnectionFactory
            {
                HostName="192.168.1.97",
                UserName="meteonel",
                Password = "7Z*0f4QRHOuO",
                VirtualHost = "meteonel"
            };

            using (var connection = factory.CreateConnection())
            {
                var chargeIngestor = new ChargeIngestor(_sessionFactory);
                var rainTipIngestor = new RainTipIngestor(_sessionFactory);
                var windIngestor = new WindIngestor(_sessionFactory);
                var ds18b20Ingestor = new Ds18B20Ingestor(_sessionFactory);
                var bme280Ingestor = new Bme280Ingestor(_sessionFactory);
                
                chargeIngestor.Ingest(connection);
                rainTipIngestor.Ingest(connection);
                windIngestor.Ingest(connection);
                ds18b20Ingestor.Ingest(connection);
                bme280Ingestor.Ingest(connection);

                Console.CancelKeyPress += OnExit;
                _closing.WaitOne();
            }
        }

        protected static void OnExit(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("Exit");
            _closing.Set();
        }
    }
}