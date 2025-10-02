namespace RaceDataApp.Reader.Domain.Entities;

public class DriverSummary
{
    public int DriverId { get; set; }

    public string DriverRef { get; set; }

    public int? Number { get; set; }

    public string Code { get; set; }

    public string Forename { get; set; }

    public string Surname { get; set; }

    public DateTime Dob { get; set; }

    public string Nationality { get; set; }

    public string Url { get; set; }
    
    public int TotalPodiums { get; set; }
    
    public int TotalRaces { get; set; }
}