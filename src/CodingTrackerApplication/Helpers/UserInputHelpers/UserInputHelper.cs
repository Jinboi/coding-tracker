// -------------------------------------------------------------------------------------------------
//  CodingTrackerApplication.Helpers.UtilityHelpers.InputHelper
// -------------------------------------------------------------------------------------------------
// Provides helper methods to gather user inputs related to start and end times. It also ensures 
// users can return to the main menu during input if needed.
// -------------------------------------------------------------------------------------------------

using System.Globalization;
using CodingTrackerApplication.Helpers.UtilityHelpers;
using CodingTrackerApplication.Models;
using CodingTrackerApplication.Views;

namespace CodingTrackerApplication.Helpers.UserInputHelpers;
internal class UserInputHelper
{
    internal static string GetEndTimeInput()
    {
        Console.WriteLine("\n\nPlease insert the EndTime: (Format: 12/28/2010 12:10:15 PM). Type 0 to return to main menu");
        string endTimeInput = ConsoleHelper.ReadNonNullInput(); ;

        if (endTimeInput == "0") MainMenu.GetUserInput();

        return endTimeInput;
    }
    internal static string GetStartTimeInput()
    {
        Console.WriteLine("\n\nPlease insert the StartTime: (Format: 12/28/2010 12:10:15 PM). Type 0 to return to main menu");
        string startTimeInput = ConsoleHelper.ReadNonNullInput(); ;

        if (startTimeInput == "0") MainMenu.GetUserInput();

        while (!DateTime.TryParseExact(startTimeInput, "MM/dd/yyyy hh:mm:ss tt", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\n\nInvalid date. (Format: 12/28/2010 12:10:15 PM). Type 0 to return to main menu or try again.\n\n");
            startTimeInput = ConsoleHelper.ReadNonNullInput(); ;
        }

        return startTimeInput;
    }

    internal static int getUserIdPrompt()
    {
        Console.WriteLine("Enter User ID:");
        int userId = int.Parse(Console.ReadLine() ?? "0");
        return userId;
    }
    internal static string getFilteringPeriodInputs()
    {
        Console.WriteLine("Choose a period for filtering: (days/weeks/years/all)");
        string period = Console.ReadLine() ?? "all";
        return period;
    }

    internal static string getFilteringOrderInputs()
    {
        Console.WriteLine("Choose sorting order: (asc/desc)");
        string order = Console.ReadLine() ?? "asc";
        return order;
    }
    internal static CodingSession getUserInputForStopwatchSession()
    {
        // Create a new instance of CodingSession
        CodingSession session = new CodingSession();

        // Ask the user for their ID
        Console.WriteLine("Enter your User ID:");
        session.Id = int.Parse(Console.ReadLine() ?? "0");

        Console.WriteLine("Press Enter to start the stopwatch...");
        Console.ReadLine();
        session.StartTime = DateTime.Now;
        Console.WriteLine($"Started at: {session.StartTime}");

        Console.WriteLine("Press Enter to stop the stopwatch...");
        Console.ReadLine();
        session.EndTime = DateTime.Now;
        Console.WriteLine($"Stopped at: {session.EndTime}");

        double durationDouble = TimeHelper.CalculateDuration(session.StartTime, session.EndTime);
        session.Duration = Convert.ToInt32(durationDouble);

        return session;
    }
}
