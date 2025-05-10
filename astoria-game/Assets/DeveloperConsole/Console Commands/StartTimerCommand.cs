namespace Console.Commands
{
    public class _StartTimer : ConsoleCommand
    {
        public override string Command => "starttimer";

        public override string WrongInputMessage => "This command doesn't take any inputs.";

        public override string SuccessMessage => "Starting timer";

        public override bool Process(string[] args) {
            if (args.Length > 0) return false;
            OGPCController.Instance.StartTimer();
            return true;
        }
    }
}
