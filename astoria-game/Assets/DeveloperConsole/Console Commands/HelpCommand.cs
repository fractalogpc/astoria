namespace Console.Commands
{
    public class HelpCommand : ConsoleCommand
    {
        public override string Command => "help";

        public override string WrongInputMessage => "This command doesn't take any inputs";

        public override string SuccessMessage => string.Empty;

        public override bool Process(string[] args) {
            if (args.Length > 0) return false;
            ConsoleUI._consoleHelp?.Invoke();
            return true;
        }
    }
}
