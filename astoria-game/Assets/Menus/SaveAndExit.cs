using UnityEngine;

public class SaveAndExit : MonoBehaviour
{

    public void SaveAndExitGame()
    {
        // Save the game state
        SaveSystem.Instance.SaveGame();
        
        // Load the main menu scene
        GameState.Instance.EndGame();
    }

}
