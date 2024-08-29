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
    public void CreateRecord()
    {
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

        _codingTrackerService.Create(startTime, endTime, duration);
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
        _codingTrackerService.Update(recordId, startTime, endTime, duration);
    }
}
