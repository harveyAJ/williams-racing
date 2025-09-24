using RaceDataApp.Reader.Domain.Entities;
using ServiceStack;
using RaceDataApp.Reader.ServiceModel;
using ServiceStack.Logging;
using ServiceStack.Text;

namespace RaceDataApp.Reader.ServiceInterface;

public class RaceDataQueryService(ICommandExecutor cmdExecutor) : Service
{
    private readonly ILog _logger = LogManager.GetLogger(typeof(RaceDataQueryService));
    
    public async Task<List<CircuitSummary>> GetAsync(CircuitSummaryRequest request)
    {
        try
        {
            _logger.Info($"GetAsync called {request.Dump()}");
            var cmd = cmdExecutor.Command<IAsyncCommand<CircuitSummaryRequest, List<CircuitSummary>>>();
            await cmd.ExecuteAsync(request);
            return cmd.Result;
        }
        catch (Exception e)
        {
            _logger.Error(e);
            throw;
        }
    }
    
    public async Task<List<DriverSummary>> GetAsync(DriverSummaryRequest request)
    {
        try
        {
            _logger.Info($"GetAsync called {request.Dump()}");
            var cmd = cmdExecutor.Command<IAsyncCommand<DriverSummaryRequest, List<DriverSummary>>>();
            await cmd.ExecuteAsync(request);
            return cmd.Result;
        }
        catch (Exception e)
        {
            _logger.Error(e);
            throw;
        }
    }
}