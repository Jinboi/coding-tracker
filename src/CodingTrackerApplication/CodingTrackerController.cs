using CodingTrackerApplication.Helpers;
using System.Globalization;

namespace CodingTrackerApplication;
internal class CodingTrackerController
{
    private readonly CodingTrackerService _codingTrackerService = new();
    public void ViewAllRecords()
    {
        var records = _codingTrackerService.GetAllRecords();

        Console.WriteLine("----------------------------------------------------\n");
        foreach (var record in records)
        {
            Console.WriteLine($"{record.Id} - {record.StartTime.ToString("yyyy-MM-dd HH:mm")} - {record.EndTime.ToString("yyyy-MM-dd HH:mm")} - {record.Duration} minutes");
        }
        Console.WriteLine("----------------------------------------------------\n");
    }
    public void ViewFilteredRecords()
    {
        Console.WriteLine("Choose a period for filtering: (days/weeks/years/all)");
        string period = Console.ReadLine() ?? "all";

        Console.WriteLine("Choose sorting order: (asc/desc)");
        string order = Console.ReadLine() ?? "asc";

        var records = _codingTrackerService.GetFilteredRecords(period, order);

        Console.WriteLine("----------------------------------------------------\n");
        foreach (var record in records)
        {
            Console.WriteLine($"{record.Id} - {record.StartTime.ToString("yyyy-MM-dd HH:mm")} - {record.EndTime.ToString("yyyy-MM-dd HH:mm")} - {record.Duration} minutes");
        }
        Console.WriteLine("----------------------------------------------------\n");
    }
    public void CreateRecord()
    {

        Console.WriteLine("Would you like to track the session using a stopwatch? (y/n)");
        string useStopwatch = Console.ReadLine()?.Trim().ToLower() ?? string.Empty;

        if (useStopwatch == "y")
        {
            StartStopwatchSession();
        }
        else
        {
            ManualEntrySession();
        }
    }
    private void ManualEntrySession()
    {
        // Ask the user for their ID
        Console.WriteLine("Enter your User ID:");
        int userId = int.Parse(Console.ReadLine() ?? "0");

        string startTimeInput = InputHelper.GetStartTimeInput();
        string endTimeInput = InputHelper.GetEndTimeInput();

        CultureInfo culture = new CultureInfo("en-US");
        DateTime startTime = Convert.ToDateTime(startTimeInput, culture);
        DateTime endTime = Convert.ToDateTime(endTimeInput, culture);

        double durationDouble = TimeHelper.CalculateDuration(startTime, endTime);
        int duration = Convert.ToInt32(durationDouble);

        if (duration < 0)
        {
            Console.WriteLine("Duration cannot be negative. Type 0 to Start from scratch");
            string goBackToMainMenu = ConsoleHelper.ReadNonNullInput();
            if (goBackToMainMenu == "0") MainMenu.GetUserInput();
        }

        _codingTrackerService.Create(userId, startTime, endTime, duration);

        // Update progress
        ViewGoalProgress();
    }
   private void StartStopwatchSession()
{
    // Ask the user for their ID
    Console.WriteLine("Enter your User ID:");
    int userId = int.Parse(Console.ReadLine() ?? "0");

    Console.WriteLine("Press Enter to start the stopwatch...");
    Console.ReadLine();
    DateTime startTime = DateTime.Now;
    Console.WriteLine($"Started at: {startTime}");

    Console.WriteLine("Press Enter to stop the stopwatch...");
    Console.ReadLine();
    DateTime endTime = DateTime.Now;
    Console.WriteLine($"Stopped at: {endTime}");

    double durationDouble = TimeHelper.CalculateDuration(startTime, endTime);
    int duration = Convert.ToInt32(durationDouble);

    _codingTrackerService.Create(userId, startTime, endTime, duration);
    Console.WriteLine($"Session recorded: {duration} minutes");

    // Update progress
    ViewGoalProgress();
}
    internal void DeleteRecord()
    {
        Console.Clear();
        ViewAllRecords();

        var recordId = Validation.GetNumberInput("\n\nPlease type the Id of the record you want to delete ot type 0 to back to Main Menu\n\n");

        _codingTrackerService.Delete(recordId);

        Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");

        MainMenu.GetUserInput();
    }
    internal void UpdateRecord()
    {
        Console.Clear();
        ViewAllRecords();

        var recordId = Validation.GetNumberInput("\n\nPlease type Id of the record you would like to update. Type 0 to go back to Main Menu.\n\n");


        // Ask the user for their ID
        Console.WriteLine("Enter your User ID:");
        int userId = int.Parse(Console.ReadLine() ?? "0");

        string startTimeInput = InputHelper.GetStartTimeInput();
        string endTimeInput = InputHelper.GetEndTimeInput();

        CultureInfo culture = new CultureInfo("en-US");
        DateTime startTime = Convert.ToDateTime(startTimeInput, culture);
        DateTime endTime = Convert.ToDateTime(endTimeInput, culture);

        double durationDouble = TimeHelper.CalculateDuration(startTime, endTime);
        int duration = Convert.ToInt32(durationDouble);

        if (duration < 0)
        {
            Console.WriteLine("Duration cannot be negative. Type 0 to Start from scartch");
            string goBacktoMainMenu = ConsoleHelper.ReadNonNullInput();
            if (goBacktoMainMenu == "0") MainMenu.GetUserInput();
        }

        _codingTrackerService.Update(recordId, userId, startTime, endTime, duration);
    }
    public void GenerateReport()
    {
        Console.WriteLine("Choose a period for the report: (days/weeks/years/all)");
        string period = Console.ReadLine()?.ToLower() ?? "all";

        var report = _codingTrackerService.GetSessionReport(period);

        Console.WriteLine("----------------------------------------------------\n");
        Console.WriteLine($"Total Coding Duration: {report.totalDuration} minutes");
        Console.WriteLine($"Average Coding Duration: {report.averageDuration} minutes");
        Console.WriteLine("----------------------------------------------------\n");
    }
    public void SetGoal()
    {
        Console.WriteLine("Enter User ID:");
        int userId = int.Parse(Console.ReadLine() ?? "0");

        Console.WriteLine("Enter Goal Amount (in minutes):");
        int goalAmount = int.Parse(Console.ReadLine() ?? "0");

        Console.WriteLine("Enter Start Date (yyyy-MM-dd):");
        DateTime startDate = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString("yyyy-MM-dd"));

        Console.WriteLine("Enter End Date (yyyy-MM-dd):");
        DateTime endDate = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString("yyyy-MM-dd"));

        _codingTrackerService.SetGoal(userId, goalAmount, startDate, endDate);

        Console.WriteLine("Goal has been set successfully.");
    }
    public void ViewGoalProgress()
    {
        Console.WriteLine("Enter User ID:");
        int userId = int.Parse(Console.ReadLine() ?? "0");

        var progress = _codingTrackerService.GetGoalProgress(userId);

        if (progress == null)
        {
            Console.WriteLine("No goal found for this user.");
            return;
        }

        Console.WriteLine("----------------------------------------------------\n");
        Console.WriteLine($"Total Duration Logged: {progress.TotalDuration} minutes");
        Console.WriteLine($"Goal: {progress.GoalAmount} minutes");
        Console.WriteLine($"Progress: {progress.ProgressPercentage:F2}%");
        Console.WriteLine($"Daily Goal to Reach Target: {progress.DailyGoal:F2} minutes/day");
        Console.WriteLine("----------------------------------------------------\n");

        if (progress.ProgressPercentage >= 100)
        {
            Console.WriteLine("Congratulations! You've reached your goal.");
        }
        else
        {
            Console.WriteLine("Keep going! You're on your way to reaching your goal.");
        }
    }
}
