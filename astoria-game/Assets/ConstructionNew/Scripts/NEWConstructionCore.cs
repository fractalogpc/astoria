using UnityEngine;

public class NEWConstructionCore : MonoBehaviour
{
    public bool SelectData(ConstructionData data) {
        if (!ValidateData(data)) return false;

        

        return false;
    }

    private bool ValidateData(ConstructionData data) {
        if (data == null) {
            Debug.LogError("ConstructionData is null");
            return false;
        }

        return true;
    }
}
