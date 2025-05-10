namespace Console.Commands
{
    public class _ResetTimerCommand : ConsoleCommand
    {
        public override string Command => "resettimer";

        public override string WrongInputMessage => "This command doesn't take any inputs.";

        public override string SuccessMessage => "Resetting timer";

        public override bool Process(string[] args) {
            if (args.Length > 0) return false;
            OGPCController.Instance.ResetTimer();
            return true;
        }
    }
}
