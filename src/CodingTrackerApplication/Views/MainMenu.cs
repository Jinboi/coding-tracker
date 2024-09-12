// -------------------------------------------------------------------------------------------------
// CodingTrackerApplication.Views.MainMenu
// -------------------------------------------------------------------------------------------------
// Main Menu of the CodingTrackerApplication. Takes UserInputs from the menu, then proceed to the 
// CodingTrackerController.
// -------------------------------------------------------------------------------------------------

using CodingTrackerApplication.Controllers;
using CodingTrackerApplication.Helpers.UtilityHelpers;
using Spectre.Console;

namespace CodingTrackerApplication.Views;
internal interface MainMenu
{
    private static readonly CodingTrackerController _controller = new();

    private static readonly GoalController _goalController = new();
    internal static void GetUserInput()
    {
        Console.Clear();
        bool closeApp = false;
        while (closeApp == false)
        {
            AnsiConsole.Markup("[underline red]MAIN MENU[/]");
            Console.WriteLine("\nWhat would you like to do?");
            AnsiConsole.Markup("\nType 0 to [underline blue]Close[/]  Application.");
            AnsiConsole.Markup("\nType 1 to [underline blue]View[/]  All Records.");
            AnsiConsole.Markup("\nType 2 to [underline blue]Insert[/]  Records.");
            AnsiConsole.Markup("\nType 3 to [underline blue]Delete[/]  Records.");
            AnsiConsole.Markup("\nType 4 to [underline blue]Update[/]  Records.");
            AnsiConsole.Markup("\nType 5 to [underline blue]View Filtered[/]  Records.");
            AnsiConsole.Markup("\nType 6 to [underline blue]Generate Rerpot[/]");
            AnsiConsole.Markup("\nType 7 to [underline blue]Set Coding Goal[/]");
            AnsiConsole.Markup("\nType 8 to [underline blue]View Goal Progress[/]");

            Console.WriteLine("\n------------------------------------------------------\n");

            string command = ConsoleHelper.ReadNonNullInput();

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    _controller.ViewAllRecords();
                    break;
                case "2":
                    _controller.CreateRecord();
                    break;
                case "3":
                    _controller.DeleteRecord();
                    break;
                case "4":
                    _controller.UpdateRecord();
                    break;
                case "5":
                    _controller.ViewFilteredRecords();
                    break;
                case "6":
                    _controller.GenerateReport();
                    break;
                case "7":
                    _goalController.SetGoal();
                    break;
                case "8":
                    _goalController.ViewGoalProgress();
                    break;

                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
        }
    }
}
