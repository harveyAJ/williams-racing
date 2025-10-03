using ServiceStack;

namespace RaceDataApp.Loader.ServiceModel;

[Route("/race/circuit", "POST", Summary = "Saves a new circuit")]
public class SaveCircuit : IReturn<SaveCircuitResponse>
{
    public string CircuitRef { get; set; }

    public string Name { get; set; }

    public string Location { get; set; }

    public string Country { get; set; }

    public decimal Lat { get; set; }

    public decimal Lng { get; set; }

    public int Alt { get; set; }

    public string Url { get; set; }
}

public class SaveCircuitResponse
{
    public int CircuitId { get; set; }
}