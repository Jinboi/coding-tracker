using Spectre.Console;

namespace CodingTrackerApplication;
internal class MainMenu
{
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
            AnsiConsole.Markup("\nType 2 to [underline blue]Insert[/]  Record.");
            AnsiConsole.Markup("\nType 3 to [underline blue]Delete[/]  Record.");
            AnsiConsole.Markup("\nType 4 to [underline blue]Update[/]  Record.");
            Console.WriteLine("\n------------------------------------------------------\n");

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    AppEngine.GetAllRecords();
                    break;
                case "2":
                    AppEngine.InsertTime();
                    break;
                case "3":
                    AppEngine.Delete();
                    break;
                case "4":
                    AppEngine.Update();
                    break;

                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
        }
    }
}
