using ServiceStack;

namespace RaceDataApp.Loader.ServiceModel;

[Route("/race/save", "POST", Summary = "Saves a new race")]
public class SaveRace : IReturn<SaveRaceResponse>
{
    public int Year { get; set; }
    
    public int Round { get; set; }

    public int CircuitId { get; set; }

    public string Name { get; set; }

    public DateTime? Date { get; set; }
    
    public TimeSpan? Time { get; set; }

    public string Url { get; set; }

    public DateTime? Fp1Date { get; set; }
    
    public TimeSpan? Fp1Time { get; set; }

    public DateTime? Fp2Date { get; set; }
    
    public TimeSpan? Fp2Time { get; set; }

    public DateTime? Fp3Date { get; set; }
    
    public TimeSpan? Fp3Time { get; set; }

    public DateTime? QualiDate { get; set; }
    
    public TimeSpan? QualiTime { get; set; }

    public DateTime? SprintDate { get; set; }
    
    public TimeSpan? SprintTime { get; set; }
}

public class SaveRaceResponse
{
    public int RaceId { get; set; }
}