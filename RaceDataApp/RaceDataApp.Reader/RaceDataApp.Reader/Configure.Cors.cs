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
                services.AddCors(options => {
                    options.AddDefaultPolicy(policy => {
                        policy.WithOrigins(["*"])
                            //.AllowCredentials()
                            .WithHeaders(["Content-Type", "Allow", "Authorization"])
                            .SetPreflightMaxAge(TimeSpan.FromHours(1));
                    });
                });
                services.AddTransient<IStartupFilter, StartupFilter>();
            });
    }
    
    public class StartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) => app =>
        {
            app.UseCors();
            next(app);
        };
    }   
}