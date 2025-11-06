using System;
using System.IO;
using System.Linq;
using NHibernate.Cfg;

namespace Infrastructure.NHibernate
{
    public static class NHibernateHelper
    {
        public static Configuration LoadConfiguration(string basePath)
        {
            var cfgPath = Path.Combine(basePath, "Infrastructure", "NHibernate", "NHibernate.cfg.xml");
            if (!File.Exists(cfgPath)) throw new FileNotFoundException("NHibernate.cfg.xml no encontrado.", cfgPath);

            var cfg = new Configuration();
            // Configurar directamente desde el fichero. El NHibernate.cfg.xml contiene rutas relativas correctas
            // Configurar usando el directorio base para resolver rutas relativas
            System.Environment.CurrentDirectory = basePath;
            cfg.Configure(cfgPath);
            return cfg;
        }

        public static string ReplaceConnectionStringForLocalDb(Configuration cfg, string mdfPath)
        {
            // Construir connection string para LocalDB que adjunte el MDF
            var attach = $"Data Source=(localdb)\\MSSQLLocalDB;AttachDbFilename={mdfPath};Integrated Security=True;Connect Timeout=30;MultipleActiveResultSets=True;";
            cfg.SetProperty("connection.connection_string", attach);
            return attach;
        }
    }
}
