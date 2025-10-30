using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.IO;
using System.Linq;
using System.Data.SqlClient;

Console.WriteLine("InitializeDb: iniciado");

// Guardamos el directorio de inicio desde el cual se llamó al programa (normalmente: ./InitializeDb)
var startupDir = Directory.GetCurrentDirectory();
// Definimos la carpeta Data según la convención del plan: InitializeDb/Data
var dataDir = Path.Combine(startupDir, "Data");
if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);

// Usar |DataDirectory| en la cadena de conexión para que AttachDBFilename apunte a InitializeDb/Data/ProjectDatabase.mdf
AppDomain.CurrentDomain.SetData("DataDirectory", dataDir);

var baseDir = AppContext.BaseDirectory;
// Ensure relative paths in NHibernate.cfg.xml are resolved against the output folder where we copied the mappings
Directory.SetCurrentDirectory(baseDir);

// try to find NHibernate.cfg.xml in various locations
var candidates = new[] {
    Path.Combine(baseDir, "NHibernate.cfg.xml"),
    Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "NHibernate", "NHibernate.cfg.xml")
};

string? cfgPath = null;
foreach(var c in candidates)
{
    if (File.Exists(c)) { cfgPath = c; break; }
}

if (cfgPath is null)
{
    Console.WriteLine("No se encontró NHibernate.cfg.xml. Coloque el archivo en InitializeDb o en Infrastructure/NHibernate/");
    return;
}

var cfg = new Configuration();
cfg.Configure(cfgPath);

Console.WriteLine($"Usando cfg: {cfgPath}");

var apply = args != null && args.Length > 0 && args.Any(a => a.Equals("apply", StringComparison.OrdinalIgnoreCase));

var export = new SchemaExport(cfg);
// If --apply (pass argument "apply"), execute against DB; otherwise only print SQL.
if (!apply)
{
    export.Execute(useStdOut: true, execute: false, justDrop: false);
    Console.WriteLine("SchemaExport mostrado en consola. No se aplicaron cambios a la base (modo seguro). Pasa el argumento 'apply' para ejecutar en DB.");
}
else
{
    // En modo apply: aseguramos que exista la base de datos LocalDB y el fichero MDF en InitializeDb/Data
    var dbName = "ProjectDatabase";
    var mdfPath = Path.Combine(dataDir, dbName + ".mdf");

    // Intentar crear la base de datos en la instancia LocalDB si no existe
    var masterConn = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=master;";
    try
    {
        using var conn = new SqlConnection(masterConn);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT db_id('{dbName}')";
        var exists = cmd.ExecuteScalar();
        if (exists == DBNull.Value || exists == null)
        {
            Console.WriteLine($"Creando base de datos '{dbName}' y fichero MDF en: {mdfPath}");
            Directory.CreateDirectory(Path.GetDirectoryName(mdfPath) ?? dataDir);
            cmd.CommandText = $"CREATE DATABASE [{dbName}] ON (NAME = N'{dbName}', FILENAME = N'{mdfPath}')";
            cmd.ExecuteNonQuery();
            Console.WriteLine("Base de datos creada.");
        }
        else
        {
            Console.WriteLine($"Base de datos '{dbName}' ya existe en la instancia LocalDB.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"No se pudo crear o comprobar la base en LocalDB: {ex.Message}");
        Console.WriteLine("Asegúrate de que LocalDB está instalado y la instancia (localdb)\\MSSQLLocalDB está disponible.");
        throw;
    }

    // Actualizamos la cadena de conexión NHibernate para que use AttachDBFilename apuntando al MDF dentro del repo
    var attachConn = $"Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True;Initial Catalog={dbName};AttachDBFilename=|DataDirectory|\\{dbName}.mdf;";
    cfg.SetProperty("connection.connection_string", attachConn);

    // Ejecutar únicamente la creación del esquema (no seed)
    export = new SchemaExport(cfg);
    export.Execute(useStdOut: true, execute: true, justDrop: false);
    Console.WriteLine("SchemaExport ejecutado contra la base de datos (solo creación de esquema). No se ejecutó seed automático.");
}

Console.WriteLine("InitializeDb completado.");
