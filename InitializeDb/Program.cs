using System;
using System.IO;
using NHibernate.Tool.hbm2ddl;
using Infrastructure.NHibernate;

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
				repoRoot = parent?.FullName;
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
				Console.WriteLine("SchemaExport completado en LocalDB. InitializeDb completado.");
				return 0;
			}
			catch (Exception e)
			{
				Console.WriteLine("Error en InitializeDb: " + e);
				return 1;
			}
		}
	}
}
