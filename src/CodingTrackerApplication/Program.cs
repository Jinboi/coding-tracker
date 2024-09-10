    // -------------------------------------------------------------------------------------------------
    // CodingTrackerApplication.Program
    // -------------------------------------------------------------------------------------------------
    // Insert point of the application. Sets up tables if they don't exist. Then, proceeds to MainMenu.
    // -------------------------------------------------------------------------------------------------

    using Microsoft.Data.Sqlite;
    using System.Configuration;

    namespace CodingTrackerApplication;
    public static class Program
    {
        static readonly string? connectionString = ConfigurationManager.AppSettings.Get("connectionString");
        public static void Main(string[] args)
        {
            CreateDatabase();       
            MainMenu.GetUserInput();
        }
        private static void CreateDatabase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                // Existing table creation with UserId added
                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS coding_session (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,                        
                        UserId INTEGER NOT NULL,
                        StartTime TEXT,
                        EndTime TEXT,
                        Duration INTEGER
                        )";

                tableCmd.ExecuteNonQuery();

                // New table creation for CodingGoals
                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS CodingGoals (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            UserId INTEGER NOT NULL,
                            GoalAmount INTEGER NOT NULL,
                            StartDate TEXT NOT NULL,
                            EndDate TEXT NOT NULL
                            )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
    }   