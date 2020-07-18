using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Meteonel.DomainModel;
using NHibernate;

namespace Meteonel
{
    public class FactoryManager
    {
        public ISessionFactory Instance { get;  }

        public FactoryManager()
        {
            const string connectionString = "Server=mdbneuinsl.mariadb.database.azure.com; Port=3306; Database=meteonel; Uid=meteonel_user@mdbneuinsl; Pwd=o1ZO73&jc4Rp";

            Instance = Fluently.Configure()
                .Database((MySQLConfiguration.Standard).ConnectionString(connectionString))
                .Mappings(m => m.AutoMappings.Add(
                    AutoMap.AssemblyOf<Device>().Where(t => t.Namespace == "Meteonel.DomainModel")))
                .BuildSessionFactory();
        }
    }
}