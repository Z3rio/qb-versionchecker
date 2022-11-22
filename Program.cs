using VersionChecker.Commands;
using static VersionChecker.Commands.ListCmd;

namespace VersionChecker
{
    public static class Program
    {
        static void MainLoop()
        {
            Console.Clear();
            Console.WriteLine("Input any valid menu option");
            Console.WriteLine("list - See a list of outdated resources\n" +
                "update - Update all or specific resource");

            string? chosenOption = Console.ReadLine();

            if (chosenOption != null)
            {
                switch (chosenOption)
                {
                    case "list":
                        ListCmd.Run();
                        break;
                    default:
                        Console.WriteLine("Incorrect input");
                        break;
                }
            } 
            else
            {
                Console.WriteLine("Incorrect input");
            }

            Console.WriteLine("\n\nPress enter to start over");
            Console.ReadLine();
            MainLoop();
        }

        public static void Main(string[] args)
        {
            Console.WriteLine(" /$$    /$$                              /$$                          \r\n| $$   | $$                             |__/                          \r\n| $$   | $$ /$$$$$$   /$$$$$$   /$$$$$$$ /$$  /$$$$$$  /$$$$$$$       \r\n|  $$ / $$//$$__  $$ /$$__  $$ /$$_____/| $$ /$$__  $$| $$__  $$      \r\n \\  $$ $$/| $$$$$$$$| $$  \\__/|  $$$$$$ | $$| $$  \\ $$| $$  \\ $$      \r\n  \\  $$$/ | $$_____/| $$       \\____  $$| $$| $$  | $$| $$  | $$      \r\n   \\  $/  |  $$$$$$$| $$       /$$$$$$$/| $$|  $$$$$$/| $$  | $$      \r\n    \\_/    \\_______/|__/      |_______/ |__/ \\______/ |__/  |__/      \r\n                                                                      \r\n                                                                      \r\n                                                                      \r\n  /$$$$$$                        /$$                         /$$      \r\n /$$__  $$                      | $$                        | $$      \r\n| $$  \\__/  /$$$$$$  /$$$$$$$  /$$$$$$    /$$$$$$   /$$$$$$ | $$      \r\n| $$       /$$__  $$| $$__  $$|_  $$_/   /$$__  $$ /$$__  $$| $$      \r\n| $$      | $$  \\ $$| $$  \\ $$  | $$    | $$  \\__/| $$  \\ $$| $$      \r\n| $$    $$| $$  | $$| $$  | $$  | $$ /$$| $$      | $$  | $$| $$      \r\n|  $$$$$$/|  $$$$$$/| $$  | $$  |  $$$$/| $$      |  $$$$$$/| $$      \r\n \\______/  \\______/ |__/  |__/   \\___/  |__/       \\______/ |__/      \r\n                                                                      \r\n                                                                      \r\n                                                                      ");

            MainLoop();
        } 
    }
}
