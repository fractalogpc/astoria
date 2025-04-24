using UnityEngine;

public class EndCutsceneTimelineReceiver : MonoBehaviour
{
    
    public void OnEndCutscene()
    {
        // This method will be called when the cutscene ends
        Debug.Log("End cutscene transition triggered.");
        GameState.Instance.UnloadCutsceneEndTransitionScene();
    }

}
