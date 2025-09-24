using ServiceStack.DataAnnotations;

namespace RaceDataApp.Loader.Entities;

public class LapTime
{
    [ForeignKey(typeof(Race), OnDelete = "CASCADE", OnUpdate = "CASCADE")]
    public int RaceId { get; set; }

    [ForeignKey(typeof(Driver), OnDelete = "CASCADE", OnUpdate = "CASCADE")]
    public int DriverId { get; set; }

    public int Lap { get; set; }

    public int Position { get; set; }

    public string Time { get; set; }   // keep as string since it’s not always strict TimeSpan
        
    public int Milliseconds { get; set; }
}