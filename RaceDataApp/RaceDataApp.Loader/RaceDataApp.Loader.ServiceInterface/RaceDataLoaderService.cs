using ServiceStack;
using RaceDataApp.Loader.ServiceModel;

namespace RaceDataApp.Loader.ServiceInterface;

//These are just stubs for now, but that's how we'd add endpoints to the service
public class RaceDataLoaderService : Service
{
    public object Post(SaveDriver request)
    {
        return new SaveDriverResponse { DriverId = 123 };
    }

    public object Post(SaveCircuit request)
    {
        return new SaveCircuitResponse { CircuitId = 123 };
    }

    public object Post(SaveRace request)
    {
        return new SaveRaceResponse { RaceId = 123 };
    }
}