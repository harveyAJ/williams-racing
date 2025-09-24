[assembly: HostingStartup(typeof(AppHost))]

namespace RaceDataApp.Loader;

public class AppHost() : AppHostBase("RaceDataApp.Loader"), IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices(services => {
            // Configure ASP.NET Core IOC Dependencies
        });

    public override void Configure()
    {
        // Configure ServiceStack, Run custom logic after ASP.NET Core Startup
        SetConfig(new HostConfig {
        });
    }
}