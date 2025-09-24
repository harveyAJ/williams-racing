using ServiceStack.DataAnnotations;

namespace RaceDataApp.Loader.Entities;

public class Race
{
    [PrimaryKey]
    public int RaceId { get; set; }

    public int Year { get; set; }
    public int Round { get; set; }

    [ForeignKey(typeof(Circuit), OnDelete = "CASCADE", OnUpdate = "CASCADE")]
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