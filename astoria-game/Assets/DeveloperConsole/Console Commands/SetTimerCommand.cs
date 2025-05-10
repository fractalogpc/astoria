using UnityEngine;

namespace Console.Commands
{
    public class _SetTimerCommand : ConsoleCommand
    {
        private const string _wrongAmountMsg = "This command takes one input.";
        
        private string _currentWrongMsg = "";
        private string _currentSuccessMessage = "";
        
        public override string Command => "settimer";

        public override string WrongInputMessage => _currentWrongMsg;

        public override string SuccessMessage => _currentSuccessMessage;

        public override bool Process(string[] args) {
            if (args.Length == 0) {
                _currentWrongMsg = _wrongAmountMsg;
                return false;
            }
            if (args.Length != 1) {
                _currentWrongMsg = _wrongAmountMsg;
                return false;
            }

            string sceneArg = args[0];

            if (!TryType(sceneArg)) {
                _currentWrongMsg = BuildWrongType(sceneArg);
                return false;
            }

            int value = int.Parse(sceneArg);

            if (TryValue(value)) {
                _currentSuccessMessage = BuildSuccessMessage(value);
                OGPCController.Instance.SetTimer(value);
                return true;
            }
            else {
                _currentWrongMsg = BuildWrongValue();
                return false;
            }
            
            
        }

        private bool TryType(string input) {
            if (int.TryParse(input, out int value)) 
                return true;

            return false;
        }

        private bool TryValue(float value) {
            if (value >= 0) 
                return true;
            else
                return false;
        }

        private string BuildWrongType(string arg) =>
            $"Input must be an int.";

        private string BuildWrongValue() =>
            $"Input must be equal or greater then zero.";

        private string BuildCallbackMessage() =>
            $"timescale -> {Time.timeScale}";
        
        private string BuildSuccessMessage(float arg) =>
            $"Set timer to {arg} seconds.";
    }
}