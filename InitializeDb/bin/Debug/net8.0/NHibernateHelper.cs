using NHibernate;
using NHibernate.Cfg;
using System;
using System.IO;

namespace Infrastructure.NHibernate
{
    public static class NHibernateHelper
    {
        public static ISessionFactory BuildSessionFactory(string? cfgFile = null)
        {
            var configuration = new Configuration();
            var baseDir = AppContext.BaseDirectory;
            var cfgPath = cfgFile ?? Path.Combine(baseDir, "NHibernate.cfg.xml");
            if (!File.Exists(cfgPath))
            {
                // try relative to project folder
                cfgPath = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "NHibernate", "NHibernate.cfg.xml");
            }

            configuration.Configure(cfgPath);

            // Fix mapping paths if needed (when resources are files)
            // NHibernate will try to load mappings declared in cfg; if paths are relative to project, rewrite those to absolute
            // (This is a minimal helper; projects can extend to more robust rewrites.)

            return configuration.BuildSessionFactory();
        }
    }
}
