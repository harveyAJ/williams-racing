namespace RaceDataApp.Loader.Entities;

public class LapTime
{
    public int RaceId { get; set; }

    public int DriverId { get; set; }

    public int Lap { get; set; }

    public int Position { get; set; }

    public string Time { get; set; }   // keep as string since it’s not always strict TimeSpan
        
    public int Milliseconds { get; set; }
}