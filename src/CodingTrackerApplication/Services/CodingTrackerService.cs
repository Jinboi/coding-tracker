﻿// -------------------------------------------------------------------------------------------------
// CodingTrackerApplication.CodingTrackerService
// -------------------------------------------------------------------------------------------------
// Interacts with the database to perform CRUD operations (Create, Read, Update, Delete) on coding 
// session records. Also handles goal setting and progress tracking, including generating reports and 
// filtering session data based on time periods.
// -------------------------------------------------------------------------------------------------

using CodingTrackerApplication.Models;
using Microsoft.Data.Sqlite;
using System.Configuration;
using Dapper;

namespace CodingTrackerApplication.Services;
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
            connection.Execute(query, new
            {
                UserId = userId,
                StartTime = startTime,
                EndTime = endTime,
                Duration = duration
            });
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
            connection.Execute(query, new
            {
                Id = recordId,
                UserId = userId,
                StartTime = startTime,
                EndTime = endTime,
                Duration = duration
            });
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
    public Report GetSessionReport(string period)
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

            // Fetch the result as a tuple
            var result = connection.QuerySingleOrDefault<(double totalDuration, double averageDuration)>(query);

            // Return a Report object using the result
            return new Report((int)result.totalDuration, result.averageDuration);
        }
    }
    public void SetGoal(Goal goal)
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
            command.Parameters.AddWithValue("@UserId", goal.UserId);
            command.Parameters.AddWithValue("@GoalAmount", goal.GoalAmount);
            command.Parameters.AddWithValue("@StartDate", goal.StartDate.ToString("o"));
            command.Parameters.AddWithValue("@EndDate", goal.EndDate.ToString("o"));
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
            return new GoalProgress
            {
                TotalDuration = totalDuration,
                GoalAmount = goal.GoalAmount,
                ProgressPercentage = 0,
                DailyGoal = 0
            };
        }

        if (daysLeft <= 0)
        {
            return new GoalProgress
            {
                TotalDuration = totalDuration,
                GoalAmount = goal.GoalAmount,
                ProgressPercentage = CalculateProgressPercentage(totalDuration, goal.GoalAmount),
                DailyGoal = 0
            };
        }

        var dailyGoal = CalculateDailyGoal(goal.GoalAmount, totalDuration, daysLeft);
        var progressPercentage = CalculateProgressPercentage(totalDuration, goal.GoalAmount);

        return new GoalProgress
        {
            TotalDuration = totalDuration,
            GoalAmount = goal.GoalAmount,
            ProgressPercentage = progressPercentage,
            DailyGoal = dailyGoal
        };
    }

    private double CalculateProgressPercentage(int totalDuration, int goalAmount)
    {
        return goalAmount == 0 ? 0 : totalDuration / (double)goalAmount * 100;
    }

    private double CalculateDailyGoal(int goalAmount, int totalDuration, double daysLeft)
    {
        return (goalAmount - totalDuration) / daysLeft;
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