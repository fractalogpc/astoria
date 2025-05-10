namespace Console.Commands
{
    public class _StopTimerCommand : ConsoleCommand
    {
        public override string Command => "stoptimer";

        public override string WrongInputMessage => "This command doesn't take any inputs.";

        public override string SuccessMessage => "Stopping timer";

        public override bool Process(string[] args) {
            if (args.Length > 0) return false;
            OGPCController.Instance.StopTimer();
            return true;
        }
    }
}
