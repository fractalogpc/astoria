using UnityEngine;
using Construction;

public class NEWConstructionCore : InputHandlerBase
{

    protected override void InitializeActionMap() {
        RegisterAction(_inputActions.Player.Place, _ => { OnPlace(); });
    }

    private ConstructionData _selectedData;

    public bool SelectData(ConstructionData data) {
        if (!ConstructionCoreLogic.ValidateData(data)) return false;



        return true;
    }

    private void OnPlace() {
        if (_selectedData == null) {
            Debug.LogError("No data selected");
            return;
        }
    }

}

namespace Construction {
    public static class ConstructionCoreLogic {
        public static bool ValidateData(ConstructionData data) {
            if (data == null) {
                Debug.LogError("ConstructionData is null");
                return false;
            }

            return true;
        }
    }
}