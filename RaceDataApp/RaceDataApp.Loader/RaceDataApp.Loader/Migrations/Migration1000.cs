using System.Globalization;
using System.Text;
using Npgsql;
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
        ");
        SeedData();
        Db.ExecuteSql(@"
        ALTER TABLE ""lap_time"" ADD CONSTRAINT ""lap_time_pkey"" PRIMARY KEY (""race_id"", ""driver_id"", ""lap"");
        ");
        Db.ExecuteSql(@"
        ALTER TABLE ""lap_time"" DROP CONSTRAINT IF EXISTS ""FK_lap_time_race_RaceId"";
        ALTER TABLE ""lap_time"" ADD CONSTRAINT ""FK_lap_time_race_RaceId"" FOREIGN KEY (race_id) REFERENCES race(race_id) ON UPDATE CASCADE ON DELETE CASCADE;
        ALTER TABLE ""lap_time"" DROP CONSTRAINT IF EXISTS ""FK_lap_time_driver_DriverId"";
        ALTER TABLE ""lap_time"" ADD CONSTRAINT ""FK_lap_time_driver_DriverId"" FOREIGN KEY (driver_id) REFERENCES driver(driver_id) ON UPDATE CASCADE ON DELETE CASCADE;
            ");
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
        LoadDriverStandings().Wait();
        LoadLapTimes().Wait();
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

    private async Task LoadDriverStandings()
    {
        _logger.Info("Loading driver standings...");
        var npgsqlConn = (NpgsqlConnection)Db.ToDbConnection();
        if (npgsqlConn.State != System.Data.ConnectionState.Open)
        {
            await npgsqlConn.OpenAsync();
        }

        await using var writer = await npgsqlConn.BeginTextImportAsync(
            "COPY driver_standing (driver_standings_id, race_id, driver_id, points, position, position_text, wins) FROM STDIN (FORMAT CSV, HEADER true)");

        using var file = new StreamReader("./dataset/driver_standings.csv");
        while (!file.EndOfStream)
        {
            var line = await file.ReadLineAsync();
            if (!string.IsNullOrWhiteSpace(line))
            {
                await writer.WriteLineAsync(line);
            }
        }

        await writer.FlushAsync();
    }
    
    private async Task LoadLapTimes()
    {
        _logger.Info("Loading lap times...");
        var npgsqlConn = (NpgsqlConnection)Db.ToDbConnection();
        if (npgsqlConn.State != System.Data.ConnectionState.Open)
        {
            await npgsqlConn.OpenAsync();
        }

        await using var writer = await npgsqlConn.BeginTextImportAsync(
            "COPY lap_time (race_id, driver_id, lap, position, time, milliseconds) FROM STDIN (FORMAT CSV, HEADER true)");

        using var file = new StreamReader("./dataset/lap_times.csv");
        while (!file.EndOfStream)
        {
            var line = await file.ReadLineAsync();
            if (!string.IsNullOrWhiteSpace(line))
            {
                await writer.WriteLineAsync(line);
            }
        }

        await writer.FlushAsync();
    }
    
    private static DateTime? ParseDate(string value) =>
        string.IsNullOrWhiteSpace(value) || value == "\\N"
            ? null
            : DateTime.ParseExact(value.Trim('"'), "yyyy-MM-dd", CultureInfo.InvariantCulture);

    private static TimeSpan? ParseTime(string value) =>
        string.IsNullOrWhiteSpace(value) || value == "\\N"
            ? null
            : TimeSpan.Parse(value.Trim('"'), CultureInfo.InvariantCulture);
}