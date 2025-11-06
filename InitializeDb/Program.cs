using System;
using System.IO;
using NHibernate.Tool.hbm2ddl;
using Infrastructure.NHibernate;
using Microsoft.Extensions.DependencyInjection;
using ApplicationCore.Domain.CEN;
using ApplicationCore.Domain.Repositories;
using Infrastructure.NHibernate.Repositories;
using InitializeDb.Data;

namespace InitializeDb
{
	class Program
	{
		static int Main(string[] args)
		{
			Console.WriteLine("InitializeDb: iniciando creación de esquema NHibernate...");

			// Determinar raíz del repositorio buscando 'domain.model.json' hacia arriba
			string repoRoot = AppContext.BaseDirectory;
			while (repoRoot != null && !File.Exists(Path.Combine(repoRoot, "domain.model.json")))
			{
				var parent = Directory.GetParent(repoRoot);
				repoRoot = parent?.FullName ?? null;
			}
			if (repoRoot == null)
				throw new Exception("No se pudo localizar la raíz del repositorio (domain.model.json)");
			var dataDir = Path.Combine(repoRoot, "InitializeDb", "Data");
			Directory.CreateDirectory(dataDir);
			var mdfPath = Path.Combine(dataDir, "ProjectDatabase.mdf");

			try
			{
				// Cargar configuración (intenta con la connection string por defecto en NHibernate.cfg.xml)
				var cfg = NHibernateHelper.LoadConfiguration(repoRoot);

				// Intentar construir SessionFactory (puede fallar si la instancia nombrada no existe)
				try
				{
					var sf = cfg.BuildSessionFactory();
					Console.WriteLine("Conexión con la cadena por defecto OK. Ejecutando SchemaExport...");
					var export = new SchemaExport(cfg);
					export.Create(false, true);
					Console.WriteLine("SchemaExport completado usando la cadena por defecto.");
					return 0;
				}
				catch (Exception ex)
				{
					Console.WriteLine("Fallo al conectar con la cadena por defecto: " + ex.Message);
					Console.WriteLine("Aplicando fallback a LocalDB y reintentando...");
				}

				// Fallback: usar LocalDB con AttachDbFilename
				NHibernateHelper.ReplaceConnectionStringForLocalDb(cfg, mdfPath);
				var sf2 = cfg.BuildSessionFactory();
				Console.WriteLine("Conexión LocalDB establecida. Ejecutando SchemaExport...");
				var export2 = new SchemaExport(cfg);

				// Si el fichero existe y produce errores por esquema incompatible, lo eliminamos para recrear limpio
				if (File.Exists(mdfPath))
				{
					try
					{
						File.Delete(mdfPath);
						Console.WriteLine("MDF existente eliminado para recrear esquema limpio.");
					}
					catch (Exception)
					{
						Console.WriteLine("No se pudo eliminar MDF existente; SchemaExport intentará adjuntarlo de todos modos.");
					}
				}

				export2.Create(false, true);
				Console.WriteLine("SchemaExport completado en LocalDB.");

                // Configurar servicios para seeding
                var services = ConfigureServices(cfg);
                using (var scope = services.CreateScope())
                {
                    try
                    {
                        var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
                        seeder.SeedData();
                        Console.WriteLine("Seeding de datos completado correctamente.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error durante el seeding de datos: {ex.Message}");
                        return 1;
                    }
                }

                Console.WriteLine("InitializeDb completado correctamente.");
				return 0;
			}
			catch (Exception e)
			{
				Console.WriteLine("Error en InitializeDb: " + e);
				return 1;
			}
		}

        private static ServiceProvider ConfigureServices(NHibernate.Cfg.Configuration nhConfig)
        {
            var services = new ServiceCollection();

            // Configuración de NHibernate
            services.AddSingleton(nhConfig);
            services.AddSingleton(provider => provider.GetRequiredService<NHibernate.Cfg.Configuration>().BuildSessionFactory());
            services.AddScoped(provider => provider.GetRequiredService<NHibernate.ISessionFactory>().OpenSession());

            // Registro de UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Registro de Repositorios
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
			services.AddScoped<IPeliculaRepository, PeliculaRepository>();
			services.AddScoped<IResenyaRepository, ResenyaRepository>();
            services.AddScoped<IListaRepository, ListaRepository>();
            services.AddScoped<IReporteRepository, ReporteRepository>();
            services.AddScoped<INotificacionRepository, NotificacionRepository>();
            services.AddScoped<IMetricaRepository, MetricaRepository>();

            // Registro de CENs
            services.AddScoped<UsuarioCEN>();
            services.AddScoped<PeliculaCEN>();
			services.AddScoped<ResenyaCEN>();
            services.AddScoped<ListaCEN>();
            services.AddScoped<ReporteCEN>();
            services.AddScoped<NotificacionCEN>();
            services.AddScoped<MetricaCEN>();

            // Registro del DataSeeder
            services.AddScoped<DataSeeder>();

            return services.BuildServiceProvider();
        }
	}
}
