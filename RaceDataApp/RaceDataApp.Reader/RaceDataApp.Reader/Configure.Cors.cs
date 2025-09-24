[assembly: HostingStartup(typeof(ConfigureCors))]

namespace RaceDataApp.Reader;

public class ConfigureCors : IHostingStartup
{
    /// <summary>
    ///     Configure the CORS
    /// </summary>
    /// <param name="builder">The web host builder</param>
    public void Configure(IWebHostBuilder builder)
    {
        builder
            .ConfigureServices(services =>
            {
                services.AddPlugin(new CorsFeature(allowCredentials: true, allowedOrigins: "http://localhost:4200",
                    allowedMethods: "GET, POST, PUT, DELETE, OPTIONS",
                    allowedHeaders: "Content-Type, Authorization, Access-Control-Allow-Origin"));
            });
    }
}