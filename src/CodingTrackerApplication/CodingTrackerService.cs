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
    public void Create(int userId, DateTime startTime, DateTime endTime, int duration)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            var query = @"INSERT INTO coding_session (UserId, StartTime, EndTime, Duration) 
                        VALUES (@UserId, @StartTime, @EndTime, @Duration)";
            connection.Execute(query, new { UserId = userId, StartTime = startTime, EndTime = endTime, 
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
    public void Update(int recordId, int userId, DateTime startTime, DateTime endTime, int duration)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            var query = @"UPDATE coding_session SET UserId = @UserId, StartTime = @StartTime, EndTime = @EndTime, 
                        Duration = @Duration WHERE Id = @Id";
            connection.Execute(query, new { Id = recordId, UserId = userId, StartTime = startTime, 
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

    public void SetGoal(int userId, int goalAmount, DateTime startDate, DateTime endDate)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using var command = connection.CreateCommand();
            // Reset goal progress if a new goal is set
            command.CommandText = @"
                DELETE FROM CodingGoals WHERE UserId = @UserId;
                INSERT INTO CodingGoals (UserId, GoalAmount, StartDate, EndDate)
                VALUES (@UserId, @GoalAmount, @StartDate, @EndDate)";
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@GoalAmount", goalAmount);
            command.Parameters.AddWithValue("@StartDate", startDate.ToString("o"));
            command.Parameters.AddWithValue("@EndDate", endDate.ToString("o"));
            command.ExecuteNonQuery();
        }
    }
    public GoalProgress GetGoalProgress(int userId)
    {
        var totalDuration = GetTotalDurationForGoal(userId);
        var goal = GetGoal(userId);

        if (goal == null) return null;

        var totalDays = (goal.EndDate - goal.StartDate).TotalDays;
        var daysLeft = (goal.EndDate - DateTime.Now).TotalDays;

        if (goal.GoalAmount == 0)
        {
            Console.WriteLine("Goal Amount is zero. Cannot calculate progress.");
            return new GoalProgress
            {
                TotalDuration = totalDuration,
                GoalAmount = goal.GoalAmount,
                ProgressPercentage = 0,
                DailyGoal = 0
            };
        }

        // Handle case where there are no remaining days
        if (daysLeft <= 0)
        {
            return new GoalProgress
            {
                TotalDuration = totalDuration,
                GoalAmount = goal.GoalAmount,
                ProgressPercentage = (totalDuration / goal.GoalAmount) * 100,
                DailyGoal = 0
            };
        }

        var dailyGoal = (goal.GoalAmount - totalDuration) / daysLeft;
        var progressPercentage = (totalDuration / goal.GoalAmount) * 100;

        Console.WriteLine($"Daily Goal: {dailyGoal}");
        Console.WriteLine($"Progress Percentage: {progressPercentage}");

        return new GoalProgress
        {
            TotalDuration = totalDuration,
            GoalAmount = goal.GoalAmount,
            ProgressPercentage = progressPercentage,
            DailyGoal = dailyGoal
        };
    }
    private Goal GetGoal(int userId)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"
                    SELECT GoalAmount, StartDate, EndDate
                    FROM CodingGoals
                    WHERE UserId = @UserId";
            command.Parameters.AddWithValue("@UserId", userId);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Goal
                {
                    GoalAmount = reader.GetInt32(0),
                    StartDate = DateTime.Parse(reader.GetString(1)),
                    EndDate = DateTime.Parse(reader.GetString(2))
                };
            }
            return null;
        }
    }
    private int GetTotalDurationForGoal(int userId)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT SUM(Duration) 
                FROM coding_session
                WHERE StartTime >= (SELECT StartDate FROM CodingGoals WHERE UserId = @UserId)
                  AND EndTime <= (SELECT EndDate FROM CodingGoals WHERE UserId = @UserId)";
            command.Parameters.AddWithValue("@UserId", userId);

            var result = command.ExecuteScalar();
            return result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }
    }
}