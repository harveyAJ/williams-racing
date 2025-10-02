using RaceDataApp.Loader.Migrations;
using ServiceStack.OrmLite;

Console.WriteLine("Running migrations...");

var dbFactory = new OrmLiteConnectionFactory(
    "Host=localhost;Port=5436;Database=racedb;Username=postgres;Password=postgres",
    PostgreSqlDialect.Provider);

var migrator = new Migrator(dbFactory, typeof(Migration1000).Assembly);
try
{
    var result = migrator.Run();
    Console.WriteLine("✅ Migrations completed successfully");
    return 0;
}
catch (Exception e)
{
    Console.Error.WriteLine($"❌ Migration failed: {e}");
    return 1;
}