using CodingTrackerApplication.Models;
using Microsoft.Data.Sqlite;
using System.Configuration;
using Dapper;

namespace CodingTrackerApplication;
internal class CodingTrackerService
{
    static readonly string? connectionString = ConfigurationManager.AppSettings.Get("connectionString");
    public List<CodingSession> GetAllRecords()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            var query = "SELECT * FROM coding_session";
            var records = connection.Query<CodingSession>(query).ToList();
           
            return records;        
        }
    }
    public void Create(DateTime startTime, DateTime endTime, int duration)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            var query = @"INSERT INTO coding_session (StartTime, EndTime, Duration) 
                        VALUES (@StartTime, @EndTime, @Duration)";
            connection.Execute(query, new { StartTime = startTime, EndTime = endTime, 
                Duration = duration });
        }
    }
    public void Delete(int recordId)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            var query = "DELETE FROM coding_session WHERE Id = @Id";
            connection.Execute(query, new { Id = recordId });
        }
    }
    public void Update(int recordId, DateTime startTime, DateTime endTime, int duration)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            var query = @"UPDATE coding_session SET StartTime = @StartTime, EndTime = @EndTime, 
                        Duration = @Duration WHERE Id = @Id";
            connection.Execute(query, new { Id = recordId, StartTime = startTime, 
                        EndTime = endTime, Duration = duration });
        }
    }

    public List<CodingSession> GetFilteredRecords(string period, string order)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            string query = "SELECT * FROM coding_session";

            // Filter by period
            switch (period.ToLower())
            {
                case "days":
                    query += " WHERE StartTime >= DATE('now', '-1 day')";
                    break;
                case "weeks":
                    query += " WHERE StartTime >= DATE('now', '-7 days')";
                    break;
                case "years":
                    query += " WHERE StartTime >= DATE('now', '-1 year')";
                    break;
                default:
                    break;
            }

            // Add ordering
            if (order.ToLower() == "asc")
            {
                query += " ORDER BY StartTime ASC";
            }
            else if (order.ToLower() == "desc")
            {
                query += " ORDER BY StartTime DESC";
            }

            var records = connection.Query<CodingSession>(query).ToList();
            return records;
        }
    }

    public (double totalDuration, double averageDuration) GetSessionReport(string period)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            string query = period switch
            {
                "days" => @"SELECT SUM(Duration) AS TotalDuration, AVG(Duration) AS AverageDuration 
                            FROM coding_session WHERE DATE(StartTime) = DATE('now')",

                "weeks" => @"SELECT SUM(Duration) AS TotalDuration, AVG(Duration) AS AverageDuration 
                            FROM coding_session WHERE strftime('%W', StartTime) = strftime('%W', 'now')",

                "years" => @"SELECT SUM(Duration) AS TotalDuration, AVG(Duration) AS AverageDuration 
                            FROM coding_session WHERE strftime('%Y', StartTime) = strftime('%Y', 'now')",

                _ => @"SELECT SUM(Duration) AS TotalDuration, AVG(Duration) AS AverageDuration 
                        FROM coding_session"
            };

            var result = connection.QuerySingleOrDefault<(double totalDuration, double averageDuration)>(query);

            return result;
        }
    }
}