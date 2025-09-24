[assembly: HostingStartup(typeof(ConfigureCommandsFeature))]

namespace RaceDataApp.Reader;

public class ConfigureCommandsFeature : IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices(services =>
        {
            services.AddPlugin(new CommandsFeature());
        }).ConfigureAppHost(afterAppHostInit: appHost => {
            var services = appHost.GetApplicationServices();
        });
}