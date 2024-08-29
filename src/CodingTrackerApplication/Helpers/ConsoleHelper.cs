namespace CodingTrackerApplication.Helpers;
internal class ConsoleHelper
{
    public static string ReadNonNullInput()
    {
        return Console.ReadLine() ?? string.Empty;
    }
}
