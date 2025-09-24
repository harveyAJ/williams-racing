using NLog.Web;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;

namespace RaceDataApp.Loader;

public class ConfigureNLog : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
                LogManager.LogFactory = new NLogFactory();
            }).UseNLog();
    }
}