using UnityEngine;

public class OnEnterMainMenu : MonoBehaviour
{
    public void OnSceneEnter() {
        Time.timeScale = 1f; // Reset time scale to normal when entering the main menu
        // Reset the OGPC timer
        if (OGPCController.Instance != null) {
            OGPCController.Instance.ResetTimer();
        }
        
        InputReader.Instance.SwitchInputMap(InputMap.Null);
    }
}
