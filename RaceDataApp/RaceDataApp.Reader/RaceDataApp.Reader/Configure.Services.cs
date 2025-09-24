using RaceDataApp.Reader.Domain.Entities;
using RaceDataApp.Reader.Domain.ResourceAccess;
using RaceDataApp.Reader.ServiceInterface.Commands;
using RaceDataApp.Reader.ServiceModel;
using ServiceStack.Data;
using ServiceStack.OrmLite;

[assembly: HostingStartup(typeof(ConfigureServices))]

namespace RaceDataApp.Reader;

public class ConfigureServices: IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            services.AddTransient<IAsyncCommand<DriverSummaryRequest, List<DriverSummary>>, DriverSummaryCommand>();
            services.AddTransient<IAsyncCommand<CircuitSummaryRequest, List<CircuitSummary>>, CircuitSummaryCommand>();
            services.AddSingleton<IRaceDataRepository, RaceDataRepository>();
            services.AddSingleton<IDbConnectionFactory>((IDbConnectionFactory) new OrmLiteConnectionFactory(context.Configuration.GetConnectionString("raceDb"), PostgreSqlDialect.Provider));
        });
    }
}