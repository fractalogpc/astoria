namespace Console.Commands
{
    public class _InfAmmoCommand : ConsoleCommand
    {
        private const string _wrongAmountMsg = "This command takes one input.";
        private const string _wrongInputMsg = "Must be a value of either 1 or 0.";

        private string _currentWrongMsg = "";
        private string _currentSuccessMessage = "";

        public override string Command => "infammo";

        public override string WrongInputMessage => _currentWrongMsg;

        public override string SuccessMessage => _currentSuccessMessage;

        public override bool Process(string[] args) {
            if (args.Length == 0) {
                _currentSuccessMessage = BuildCallbackMessage();
                return true;
            }
            if (args.Length != 1) {
                _currentWrongMsg = _wrongAmountMsg;
                return false;
            }           
            string arg = args[0];
            if (!TryType(arg)) {
                _currentWrongMsg = _wrongInputMsg;
                return false;
            }
            int value = int.Parse(arg);
            if (TryValue(value)) {
                if (value == 1) {
                    _currentSuccessMessage = BuildSuccessMessage("enabled");
                    BackgroundInfo._infAmmo = true;
                }
                else {
                    _currentSuccessMessage = BuildSuccessMessage("disabled");
                    BackgroundInfo._infAmmo = false;
                }

                return true;
            }
            else {
                _currentWrongMsg = _wrongInputMsg;
                return false;
            }
        }

        private bool TryType(string input) {
            if (int.TryParse(input, out int value)) 
                return true;

            return false;
        }

        private bool TryValue(float value) {
            if (value is 0 or 1) 
                return true;
            else
                return false;
        }

        private string BuildCallbackMessage() =>
            $"Infinite ammo mode is {(BackgroundInfo._infAmmo ? "enabled" : "disabled")}";
    
        private string BuildSuccessMessage(string arg) =>
            $"{arg} infinite ammo mode";
    
    }
}
