using RaceDataApp.Reader.Domain.Entities;

namespace RaceDataApp.Reader.Domain.ResourceAccess;

public interface IRaceDataRepository
{
    public Task<List<CircuitSummary>> GetCircuitSummariesAsync();
    
    public Task<List<DriverSummary>> GetDriverSummariesAsync();
}