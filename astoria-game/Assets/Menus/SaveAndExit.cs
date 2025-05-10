using UnityEngine;

public class SaveAndExit : MonoBehaviour
{

    public void SaveAndExitGame()
    {
        // Reset the OGPC timer
        if (OGPCController.Instance != null)
        {
            OGPCController.Instance.ResetTimer();
        }

        // Save the game state
        SaveSystem.Instance.SaveGame();
        
        // Load the main menu scene
        GameState.Instance.EndGame();
    }

}
