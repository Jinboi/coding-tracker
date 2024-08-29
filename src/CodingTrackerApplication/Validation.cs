using CodingTrackerApplication.Helpers;

namespace CodingTrackerApplication;
internal class Validation
{
    internal static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        string numberInput = ConsoleHelper.ReadNonNullInput();

        if (numberInput == "0") MainMenu.GetUserInput();

        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("\n\nInvalid number. Try again.\n\n");
            numberInput = ConsoleHelper.ReadNonNullInput();
        }

        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }
}
