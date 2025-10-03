using ServiceStack;

namespace RaceDataApp.Loader.ServiceModel;

[Route("/driver/save", "POST", Summary = "Saves a new driver")]
public class SaveDriver : IReturn<SaveDriverResponse>
{
    public string DriverRef { get; set; }

    public int? Number { get; set; }

    public string Code { get; set; }

    public string Forename { get; set; }

    public string Surname { get; set; }

    public DateTime Dob { get; set; }

    public string Nationality { get; set; }

    public string Url { get; set; }
}

public class SaveDriverResponse
{
    public int DriverId { get; set; }
}