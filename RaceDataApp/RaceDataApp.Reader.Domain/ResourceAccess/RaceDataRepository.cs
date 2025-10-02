using RaceDataApp.Reader.Domain.Entities;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace RaceDataApp.Reader.Domain.ResourceAccess;

public class RaceDataRepository(IDbConnectionFactory connectionFactory) : IRaceDataRepository
{
    public async Task<List<CircuitSummary>> GetCircuitSummariesAsync()
    {
        using var db = connectionFactory.OpenDbConnection();
        string sql = @"
            SELECT 
               c.circuit_id,
               c.circuit_ref,
               c.name AS circuit_name,
               c.location,
               c.country,
               c.lat,
               c.lng,
               c.alt,
               c.url,          
               MIN(l.milliseconds) AS fastest_lap_ms,         
               COUNT(DISTINCT r.race_id) AS total_races          
           FROM circuit c
           JOIN race r 
               ON c.circuit_id = r.circuit_id
           JOIN lap_time l 
               ON r.race_id = l.race_id
           GROUP BY 
               c.circuit_id, c.circuit_ref, c.name, c.location, c.country, 
               c.lat, c.lng, c.alt, c.url
           ORDER BY fastest_lap_ms ASC;
        ";
        
        var results = await db.SelectAsync<CircuitSummary>(sql);
        return results ?? [];
    }

    public async Task<List<DriverSummary>> GetDriverSummariesAsync()
    {
        using var db = connectionFactory.OpenDbConnection();
        string sql = @"  
            WITH podiums AS (
              SELECT lt.driver_id, COUNT(*) AS total_podiums
              FROM lap_time lt
              JOIN (
                SELECT race_id, MAX(lap) AS max_lap
                FROM lap_time
                GROUP BY race_id
                ) r_max
              ON lt.race_id = r_max.race_id
              AND lt.lap = r_max.max_lap
              WHERE lt.position IN (1,2,3)
              GROUP BY lt.driver_id
            ),
            race_entered AS (
              SELECT driver_id, COUNT(DISTINCT race_id) AS total_races
              FROM lap_time
              GROUP BY driver_id
            )
            SELECT d.driver_id,
                   d.driver_ref,
                   d.code,
                   d.number,
                   d.forename,
                   d.surname,
                   d.nationality,
                   d.dob,
                   COALESCE(r.total_races,0) AS total_races,
                   COALESCE(p.total_podiums,0) AS total_podiums
            FROM driver d
            LEFT JOIN race_entered r ON d.driver_id = r.driver_id
            LEFT JOIN podiums p ON d.driver_id = p.driver_id
            ORDER BY total_podiums DESC, total_races DESC;
        ";
        
        var results = await db.SelectAsync<DriverSummary>(sql);
        return results ?? [];
    }
}