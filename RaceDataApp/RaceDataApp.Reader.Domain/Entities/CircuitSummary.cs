namespace RaceDataApp.Reader.Domain.Entities;

public class CircuitSummary
{
    public int CircuitId { get; set; }
    
    public string CircuitRef { get; set; }
    
    public string Name { get; set; }

    public string Location { get; set; }

    public string Country { get; set; }

    public decimal Lat { get; set; }

    public decimal Lng { get; set; }

    public int Alt { get; set; }

    public string Url { get; set; }
    
    public int FastestLapMs { get; set; }
    
    public int TotalLaps { get; set; }
}

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