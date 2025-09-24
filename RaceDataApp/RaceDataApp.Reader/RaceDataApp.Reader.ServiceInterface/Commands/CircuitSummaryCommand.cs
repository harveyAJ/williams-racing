using RaceDataApp.Reader.Domain.Entities;
using RaceDataApp.Reader.Domain.ResourceAccess;
using RaceDataApp.Reader.ServiceModel;
using ServiceStack;
using ServiceStack.Logging;

namespace RaceDataApp.Reader.ServiceInterface.Commands;

public class CircuitSummaryCommand(IRaceDataRepository repository) : AsyncCommandWithResult<CircuitSummaryRequest, List<CircuitSummary>>
{
    private readonly ILog _logger = LogManager.GetLogger(typeof(CircuitSummaryCommand));

    protected override async Task<List<CircuitSummary>> RunAsync(CircuitSummaryRequest request, CancellationToken token)
    {
        _logger.Info("RunAsync called");
        return await repository.GetCircuitSummariesAsync();
    }
}