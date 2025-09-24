using RaceDataApp.Reader.Domain.Entities;
using RaceDataApp.Reader.Domain.ResourceAccess;
using RaceDataApp.Reader.ServiceModel;
using ServiceStack;
using ServiceStack.Logging;

namespace RaceDataApp.Reader.ServiceInterface.Commands;

public class DriverSummaryCommand(IRaceDataRepository repository) : AsyncCommandWithResult<DriverSummaryRequest, List<DriverSummary>>
{
    private readonly ILog _logger = LogManager.GetLogger(typeof(DriverSummaryCommand));
    
    protected override async Task<List<DriverSummary>> RunAsync(DriverSummaryRequest request, CancellationToken token)
    {
        _logger.Info("RunAsync called");
        return await repository.GetDriverSummariesAsync();
    }
}