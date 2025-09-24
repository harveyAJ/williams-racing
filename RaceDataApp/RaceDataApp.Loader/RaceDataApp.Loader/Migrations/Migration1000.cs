using System.Globalization;
using System.Text;
using RaceDataApp.Loader.Entities;
using ServiceStack.DataAnnotations;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.PostgreSQL;

namespace RaceDataApp.Loader.Migrations;

public class Migration1000 : MigrationBase
{
    private readonly ILog _logger = LogManager.GetLogger(typeof(Migration1000));

    public override void Up()
    {
        _logger.Info("Creating the tables...");
        Db.CreateTableIfNotExists<Circuit>();
        Db.CreateTableIfNotExists<Driver>();
        Db.CreateTableIfNotExists<Race>();
        Db.CreateTableIfNotExists<DriverStanding>();
        Db.CreateTableIfNotExists<LapTime>();
        Db.ExecuteSql(@"
        ALTER TABLE ""lap_time"" DROP CONSTRAINT IF EXISTS ""lap_time_pkey"";
        ALTER TABLE ""lap_time"" ADD CONSTRAINT ""lap_time_pkey"" PRIMARY KEY (""race_id"", ""driver_id"", ""lap"");
        ");
        SeedData();
    }

    public override void Down()
    {
        _logger.Info("Deleting the tables...");
        Db.DropTable<LapTime>();
        Db.DropTable<DriverStanding>();
        Db.DropTable<Race>();
        Db.DropTable<Driver>();
        Db.DropTable<Circuit>();
    }

    private void SeedData()
    {
        _logger.Info("Seeding data...");
        LoadCircuits();
        LoadDrivers();
        LoadRaces();
        LoadDriverStandings();
        LoadLapTimes();
    }

    private void LoadCircuits()
    {
        _logger.Info("Loading circuits...");
        using var reader = new StreamReader("dataset/circuits.csv");
        string? headerLine = reader.ReadLine(); // skip header
        if (headerLine == null) return;

        var circuits = new List<Circuit>();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var parts = line.Split(',');

            circuits.Add(new Circuit
            {
                CircuitId   = int.Parse(parts[0]),
                CircuitRef  = parts[1],
                Name        = parts[2],
                Location    = parts[3],
                Country     = parts[4],
                Lat         = decimal.Parse(parts[5], CultureInfo.InvariantCulture),
                Lng         = decimal.Parse(parts[6], CultureInfo.InvariantCulture),
                Alt         = int.Parse(parts[7]),
                Url         = parts[8]
            });
        }
        
        Db.SaveAll(circuits);
    }

    private void LoadDrivers()
    {
        _logger.Info("Loading drivers...");
        using var reader = new StreamReader("./dataset/drivers.csv");
        var headerLine = reader.ReadLine(); // skip the header
        if (headerLine == null) return; //probably throw?

        var drivers = new List<Driver>();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var parts = line.Split(',');

            try
            {
                drivers.Add(new Driver
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
                });
            }
            catch (Exception e)
            {
                _logger.Error($"Error parsing drivers... {line} with exception {e.Message}");
                throw;
            }
            
        }
        
        Db.SaveAll(drivers);
    }

    private void LoadRaces()
    {
        _logger.Info("Loading races...");
        using var reader = new StreamReader("./dataset/races.csv");
        var headerLine = reader.ReadLine(); // skip the header
        if (headerLine == null) return; //probably throw?

        var races = new List<Race>();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var parts = line.Split(',');

            try
            {
                races.Add(new Race
                {
                    RaceId      = int.Parse(parts[0]),
                    Year        = int.Parse(parts[1]),
                    Round       = int.Parse(parts[2]),
                    CircuitId   = int.Parse(parts[3]),
                    Name        = parts[4].Trim('"'),
                    Date        = ParseDate(parts[5]),
                    Time        = ParseTime(parts[6]),
                    Url         = parts[7].Trim('"'),

                    Fp1Date     = ParseDate(parts[8]),
                    Fp1Time     = ParseTime(parts[9]),
                    Fp2Date     = ParseDate(parts[10]),
                    Fp2Time     = ParseTime(parts[11]),
                    Fp3Date     = ParseDate(parts[12]),
                    Fp3Time     = ParseTime(parts[13]),
                    QualiDate   = ParseDate(parts[14]),
                    QualiTime   = ParseTime(parts[15]),
                    SprintDate  = ParseDate(parts[16]),
                    SprintTime  = ParseTime(parts[17])
                });
            }
            catch (Exception e)
            {
                _logger.Error($"Error parsing race... {line} with exception {e.Message}");
                throw; //Stop the migration and everything
            }
            
        }
        
        Db.SaveAll(races);
    }

    private void LoadDriverStandings()
    {
        _logger.Info("Loading driver standings...");
        using var reader = new StreamReader("./dataset/driver_standings.csv");
        var headerLine = reader.ReadLine(); // skip the header
        if (headerLine == null) return; //probably throw?

        var races = new List<DriverStanding>();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var parts = line.Split(',');

            try
            {
                races.Add(new DriverStanding
                {
                    DriverStandingsId = int.Parse(parts[0]),
                    RaceId            = int.Parse(parts[1]),
                    DriverId          = int.Parse(parts[2]),
                    Points            = decimal.Parse(parts[3], CultureInfo.InvariantCulture),
                    Position          = parts[4] == @"\N" ? null : int.Parse(parts[4]),
                    PositionText      = parts[5].Trim('"'),
                    Wins              = int.Parse(parts[6])
                });
            }
            catch (Exception e)
            {
                _logger.Error($"Error parsing driver standings... {line} with exception {e.Message}");
                throw; //Stop the migration and everything
            }
        }
        
        Db.SaveAll(races);
    }

    private void LoadLapTimes() //That's a very large dataset using COPY FROM in Postgres (bypassing ORM) would be faster
    {
        _logger.Info("Loading lap times...");
        using var reader = new StreamReader("./dataset/lap_times.csv");
        var headerLine = reader.ReadLine(); // skip the header
        if (headerLine == null) return; //probably throw?

        var entities = new List<LapTime>();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var parts = line.Split(',');

            try
            {
                entities.Add(new LapTime
                {
                    RaceId       = int.Parse(parts[0]),
                    DriverId     = int.Parse(parts[1]),
                    Lap          = int.Parse(parts[2]),
                    Position     = int.Parse(parts[3]),
                    Time         = parts[4].Trim('"'),
                    Milliseconds = int.Parse(parts[5])
                });
            }
            catch (Exception e)
            {
                _logger.Error($"Error parsing lap times... {line} with exception {e.Message}");
                throw; //Stop the migration and everything
            }

            if (entities.Count >= BatchSize)
            {
                _logger.Info($"Batch size is reached. Inserting {BatchSize} items");
                Db.InsertAll(entities);
                entities.Clear();
            }
        }
        
        Db.InsertAll(entities); //Save the remaining
    }
    
    private const int BatchSize = 5000;
    
    private static DateTime? ParseDate(string value) =>
        string.IsNullOrWhiteSpace(value) || value == "\\N"
            ? null
            : DateTime.ParseExact(value.Trim('"'), "yyyy-MM-dd", CultureInfo.InvariantCulture);

    private static TimeSpan? ParseTime(string value) =>
        string.IsNullOrWhiteSpace(value) || value == "\\N"
            ? null
            : TimeSpan.Parse(value.Trim('"'), CultureInfo.InvariantCulture);
}