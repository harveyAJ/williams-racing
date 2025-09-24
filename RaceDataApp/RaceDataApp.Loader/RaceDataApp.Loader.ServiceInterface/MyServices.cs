using ServiceStack;
using RaceDataApp.Loader.ServiceModel;

namespace RaceDataApp.Loader.ServiceInterface;

public class MyServices : Service
{
    public object Any(Hello request)
    {
        return new HelloResponse { Result = $"Hello, {request.Name}!" };
    }
}