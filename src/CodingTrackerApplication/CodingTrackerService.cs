using CodingTrackerApplication.Models;
using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTrackerApplication;
internal class CodingTrackerService
{
    static readonly string? connectionString = ConfigurationManager.AppSettings.Get("connectionString");
    public List<CodingSession> GetAllRecords()
    {
        List<CodingSession> records = new List<CodingSession>();

        Console.Clear();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM coding_session";

            List<CodingSession> tableData = new();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                    new CodingSession
                    {
                        Id = reader.GetInt32(0),
                        StartTime = DateTime.Parse(reader.GetString(1)),
                        EndTime = DateTime.Parse(reader.GetString(2)),
                        Duration = reader.GetInt32(3)
                    });
                }
            }
            else
            {
                Console.WriteLine("No rows found");
            }

            connection.Close();

            Console.WriteLine("----------------------------------------------------\n");
            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.StartTime.ToString("yyyy-MM-dd HH:mm")} - {dw.EndTime.ToString("yyyy-MM-dd HH:mm")} - {dw.Duration} minutes");
            }
            Console.WriteLine("----------------------------------------------------\n");
        }

        return records;
    }
    public void Create(DateTime startTime, DateTime endTime, int duration)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"INSERT INTO coding_session (StartTime, EndTime, Duration) VALUES ('{startTime}', '{endTime}', '{duration}')";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void Delete(int recordId)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE from coding_session WHERE Id = '{recordId}'";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void Update(int recordId, DateTime startTime, DateTime endTime, int duration)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"UPDATE coding_session SET StartTime = '{startTime}', EndTime = '{endTime}', Duration = '{duration}' WHERE Id = {recordId}";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}