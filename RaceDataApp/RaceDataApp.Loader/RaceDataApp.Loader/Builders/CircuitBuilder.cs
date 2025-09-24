using System.Globalization;
using RaceDataApp.Loader.Entities;
using RaceDataApp.Loader.Extensions;

namespace RaceDataApp.Loader.Builders;

public interface IBuilder<out T>
{
    T Build();
}

public class CircuitBuilder(string line) : IBuilder<Circuit>
{
    public Circuit Build()
    {
        var parts = line.Split(',');

        return new Circuit
        {
            CircuitId = int.Parse(parts[0]),
            CircuitRef = parts[1],
            Name = parts[2],
            Location = parts[3],
            Country = parts[4],
            Lat = decimal.Parse(parts[5], CultureInfo.InvariantCulture),
            Lng = decimal.Parse(parts[6], CultureInfo.InvariantCulture),
            Alt = int.Parse(parts[7]),
            Url = parts[8]
        };
    }
}


public class DriverBuilder(string line) : IBuilder<Driver>
{
    public Driver Build()
    {
        var parts = line.Split(',');

        return new Driver
        {
            DriverId     = int.Parse(parts[0]),
            DriverRef    = parts[1],
            Number       = parts[2] == @"\N" ? null : int.Parse(parts[2]),
            Code         = parts[3],
            Forename     = parts[4],
            Surname      = parts[5],
            Dob          = DateTime.ParseExact(parts[6].Trim('"'), "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Nationality  = parts[7],
            Url          = parts[8]
        };
    }
}

public class RaceBuilder(string line) : IBuilder<Race>
{
    public Race Build()
    {
        var parts = line.Split(',');

        return new Race
        {
            RaceId      = int.Parse(parts[0]),
            Year        = int.Parse(parts[1]),
            Round       = int.Parse(parts[2]),
            CircuitId   = int.Parse(parts[3]),
            Name        = parts[4].Trim('"'),
            Date        = parts[5].ToDateTime(),
            Time        = parts[6].ToTimeSpan(),
            Url         = parts[7].Trim('"'),
            Fp1Date     = parts[8].ToDateTime(),
            Fp1Time     = parts[9].ToTimeSpan(),
            Fp2Date     = parts[10].ToDateTime(),
            Fp2Time     = parts[11].ToTimeSpan(),
            Fp3Date     = parts[12].ToDateTime(),
            Fp3Time     = parts[13].ToTimeSpan(),
            QualiDate   = parts[14].ToDateTime(),
            QualiTime   = parts[15].ToTimeSpan(),
            SprintDate  = parts[16].ToDateTime(),
            SprintTime  = parts[17].ToTimeSpan()
        };
    }
}

public class DriverStandingBuilder(string line) : IBuilder<DriverStanding>
{
    public DriverStanding Build()
    {
        var parts = line.Split(',');

        return new DriverStanding
        {
            DriverStandingsId = int.Parse(parts[0]),
            RaceId            = int.Parse(parts[1]),
            DriverId          = int.Parse(parts[2]),
            Points            = decimal.Parse(parts[3], CultureInfo.InvariantCulture),
            Position          = parts[4] == @"\N" ? null : int.Parse(parts[4]),
            PositionText      = parts[5].Trim('"'),
            Wins              = int.Parse(parts[6])
        };
    }
}