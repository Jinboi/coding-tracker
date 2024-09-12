// -------------------------------------------------------------------------------------------------
// CodingTrackerApplication.Helpers.UserInputHelpers.Validation
// -------------------------------------------------------------------------------------------------
// Validation class to validate UserInputs.
// -------------------------------------------------------------------------------------------------

using CodingTrackerApplication.Helpers.UtilityHelpers;
using CodingTrackerApplication.Views;

namespace CodingTrackerApplication.Helpers.UserInputHelpers;
internal class Validation
{
    internal static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        string numberInput = ConsoleHelper.ReadNonNullInput();

        if (numberInput == "0") MainMenu.GetUserInput();

        while (!int.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("\n\nInvalid number. Try again.\n\n");
            numberInput = ConsoleHelper.ReadNonNullInput();
        }

        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }
}
