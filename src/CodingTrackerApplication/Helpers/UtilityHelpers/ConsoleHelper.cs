// -------------------------------------------------------------------------------------------------
// CodingTrackerApplication.Helpers.UtilityHelpers
// -------------------------------------------------------------------------------------------------
// Provides utility functions for handling console input, ensuring non-null values are returned 
// from user input. This helper simplifies interaction with the console in the application.
// -------------------------------------------------------------------------------------------------

namespace CodingTrackerApplication.Helpers.UtilityHelpers;
internal class ConsoleHelper
{
    public static string ReadNonNullInput()
    {
        return Console.ReadLine() ?? string.Empty;
    }
    
}
