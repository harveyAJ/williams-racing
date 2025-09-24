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
           
               -- fastest lap time in milliseconds
               MIN(l.milliseconds) AS fastest_lap_ms,
           
               -- total races at this circuit
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
            SELECT
                d.driver_id,
                d.driver_ref,
                d.forename,
                d.surname,
                d.nationality,
                COUNT(CASE WHEN ds.position IN (1,2,3) THEN 1 END) AS podiums,
                COUNT(ds.race_id) AS total_races
            FROM driver d
            LEFT JOIN driver_standing ds
                ON d.driver_id = ds.driver_id
            GROUP BY
                d.driver_id, d.driver_ref, d.forename, d.surname, d.nationality
            ORDER BY podiums DESC, total_races DESC;
        ";
        
        var results = await db.SelectAsync<DriverSummary>(sql);
        return results ?? [];
    }
}