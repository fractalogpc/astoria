using UnityEngine;

public class QuitGame : MonoBehaviour
{
    
    public void Quit()
    {
        // Check if the game is running in the editor
        #if UNITY_EDITOR
            // If in the editor, stop playing the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // If in a built application, quit the game
            Application.Quit();
        #endif
    }

}
