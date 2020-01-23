using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Meteonel.Ingestor.DomainModel;
using NHibernate;

namespace Meteonel.Ingestor
{
    public class FactoryManager
    {
        public ISessionFactory Instance { get;  }

        public FactoryManager()
        {
            const string connectionString = "Server=192.168.1.97; Port=3306; Database=meteonel; Uid=meteonel_user; Pwd=cx4D&@0F#0";

            Instance = Fluently.Configure()
                .Database((MySQLConfiguration.Standard).ConnectionString(connectionString))
                .Mappings(m => m.AutoMappings.Add(
                    AutoMap.AssemblyOf<Device>().Where(t => t.Namespace == "Meteonel.Ingestor.DomainModel")))
                .BuildSessionFactory();
        }
    }
}