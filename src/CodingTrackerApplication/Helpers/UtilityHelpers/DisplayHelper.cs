// -------------------------------------------------------------------------------------------------
// CodingTrackerApplication.Helpers.UtilityHelpers.DisplayHelper
// -------------------------------------------------------------------------------------------------
// Provides static methods for displaying various types of information related to coding sessions, 
// including goal progress, report details, and records. 
// -------------------------------------------------------------------------------------------------


using CodingTrackerApplication.Constants;
using CodingTrackerApplication.Models;

namespace CodingTrackerApplication.Helpers.UtilityHelpers;
internal class DisplayHelper
{    
    internal static void DisplayGoalProgress(GoalProgress progress)
    {
        Console.WriteLine("----------------------------------------------------\n");
        Console.WriteLine($"Total Duration Logged: {progress.TotalDuration} minutes");
        Console.WriteLine($"Goal: {progress.GoalAmount} minutes");
        Console.WriteLine($"Progress: {progress.ProgressPercentage:F2}%");
        Console.WriteLine($"Daily Goal to Reach Target: {progress.DailyGoal:F2} minutes/day");
        Console.WriteLine("----------------------------------------------------\n");

        if (progress.ProgressPercentage >= 100)
        {
            Console.WriteLine(Constant.GoalReachedMessage);
        }
        else
        {
            Console.WriteLine(Constant.EncouragementMessage);
        }
    }
    internal static void displayReportInfo(Report report)
    {
        // Display the report information
        Console.WriteLine("----------------------------------------------------\n");
        Console.WriteLine($"Total Coding Duration: {report.TotalDuration} minutes");
        Console.WriteLine($"Average Coding Duration: {report.AverageDuration} minutes");
        Console.WriteLine("----------------------------------------------------\n");
    }
    internal static void displayRecordsInfo(List<CodingSession> records)
    {
        Console.WriteLine("----------------------------------------------------\n");
        foreach (var record in records)
        {
            Console.WriteLine($"{record.Id} - {record.StartTime.ToString("yyyy-MM-dd HH:mm")} - {record.EndTime.ToString("yyyy-MM-dd HH:mm")} - {record.Duration} minutes");
        }
        Console.WriteLine("----------------------------------------------------\n");
    }
}
