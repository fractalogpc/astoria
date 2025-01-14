using Unity.AppUI.UI;
using UnityEngine;

public class TestCompass : MonoBehaviour
{
    public Sprite icon;

    public bool keyItem;

    private void OnEnable() {
        CompassDisplay compassDisplay = FindFirstObjectByType<CompassDisplay>();
        if (compassDisplay == null) {
            Debug.LogError("CompassDisplay not found in scene.");
            return;
        }

        CompassIconObject compassObject = new CompassIconObject(this.gameObject, icon, keyItem);
        compassDisplay.AddObjectToCompass(compassObject);
    }
}
