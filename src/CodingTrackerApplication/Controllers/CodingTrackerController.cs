// -------------------------------------------------------------------------------------------------
// CodingTrackerApplication.CodingTrackerController
// -------------------------------------------------------------------------------------------------
// Manages user inputs and directs them to appropriate services for handling database operations 
// related to coding session tracking.
// -------------------------------------------------------------------------------------------------

using CodingTrackerApplication.Helpers.UserInputHelpers;
using CodingTrackerApplication.Helpers.UtilityHelpers;
using CodingTrackerApplication.Models;
using CodingTrackerApplication.Services;
using CodingTrackerApplication.Views;
using System.Globalization;

namespace CodingTrackerApplication.Controllers;
internal class CodingTrackerController
{
    private readonly CodingTrackerService _codingTrackerService = new();
    private readonly GoalController _goalController = new(); 
    public void ViewAllRecords()
    {
        var records = _codingTrackerService.GetAllRecords();

        DisplayHelper.displayRecordsInfo(records);
    }
    public void ViewFilteredRecords()
    {
        string period = UserInputHelper.getFilteringPeriodInputs();
        string order = UserInputHelper.getFilteringOrderInputs();

        var records = _codingTrackerService.GetFilteredRecords(period, order);

        DisplayHelper.displayRecordsInfo(records);
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
    private void StartStopwatchSession()
    {
        CodingSession session = UserInputHelper.getUserInputForStopwatchSession();

        _codingTrackerService.Create(session.Id, session.StartTime, session.EndTime, session.Duration);
        Console.WriteLine($"Session recorded: {session.Duration} minutes");

        // Update progress
        _goalController.ViewGoalProgress();
    }
    private void ManualEntrySession()
    {
        CodingSession session = getUserInputForManualSession();

        if (session.Duration < 0)
        {
            Console.WriteLine("Duration cannot be negative. Type 0 to Start from scratch");
            string goBackToMainMenu = ConsoleHelper.ReadNonNullInput();
            if (goBackToMainMenu == "0") MainMenu.GetUserInput();
        }

        _codingTrackerService.Create(session.Id, session.StartTime, session.EndTime, session.Duration);

        // Update progress
        _goalController.ViewGoalProgress();
    }
    private CodingSession getUserInputForManualSession()
    {
        CodingSession session = new CodingSession();

        // Ask the user for their ID
        Console.WriteLine("Enter your User ID:");
        session.Id = int.Parse(Console.ReadLine() ?? "0");

        string startTimeInput = UserInputHelper.GetStartTimeInput();
        string endTimeInput = UserInputHelper.GetEndTimeInput();

        CultureInfo culture = new CultureInfo("en-US");
        session.StartTime = Convert.ToDateTime(startTimeInput, culture);
        session.EndTime = Convert.ToDateTime(endTimeInput, culture);

        double durationDouble = TimeHelper.CalculateDuration(session.StartTime, session.EndTime);
        session.Duration = Convert.ToInt32(durationDouble);

        return session;
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

        string startTimeInput = UserInputHelper.GetStartTimeInput();
        string endTimeInput = UserInputHelper.GetEndTimeInput();

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
        string period = UserInputHelper.getFilteringPeriodInputs();

        // Get the report from the service
        var report = _codingTrackerService.GetSessionReport(period);

        DisplayHelper.displayReportInfo(report);
    }
}
