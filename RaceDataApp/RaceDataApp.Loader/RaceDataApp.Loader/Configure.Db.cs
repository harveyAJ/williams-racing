using ServiceStack.Data;
using ServiceStack.OrmLite;

[assembly: HostingStartup(typeof(ConfigureDb))]

namespace RaceDataApp.Loader;

public class ConfigureDb : IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices((context, service) => service.AddSingleton<IDbConnectionFactory>(
            new OrmLiteConnectionFactory(context.Configuration.GetConnectionString("raceDb"), PostgreSqlDialect.Provider)))
        .ConfigureAppHost(afterConfigure: appHost =>
        {
            appHost.ScriptContext.ScriptMethods.Add(new DbScriptsAsync());
        });
}