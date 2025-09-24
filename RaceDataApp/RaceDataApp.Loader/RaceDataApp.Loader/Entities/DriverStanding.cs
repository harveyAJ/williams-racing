using ServiceStack.DataAnnotations;

namespace RaceDataApp.Loader.Entities;

public class DriverStanding
{
    [PrimaryKey]
    public int DriverStandingsId { get; set; }

    [ForeignKey(typeof(Race), OnDelete = "CASCADE", OnUpdate = "CASCADE")]
    public int RaceId { get; set; }

    [ForeignKey(typeof(Driver), OnDelete = "CASCADE", OnUpdate = "CASCADE")]
    public int DriverId { get; set; }

    public decimal Points { get; set; }

    public int? Position { get; set; }

    public string PositionText { get; set; }

    public int Wins { get; set; }
}