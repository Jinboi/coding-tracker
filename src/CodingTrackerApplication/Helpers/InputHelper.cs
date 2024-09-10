using System.Globalization;

namespace CodingTrackerApplication.Helpers;
internal class InputHelper
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
}
