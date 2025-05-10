using UnityEngine;
using System.Collections;

public class EndCutsceneTimelineReceiver : MonoBehaviour
{

    [SerializeField] private GameObject _disableOnEndCutscene;
    [SerializeField] private float _delay = 2f;
    
    public void OnEndCutscene()
    {
        StartCoroutine(ExecuteEndCutsceneTransition());
    }

    private IEnumerator ExecuteEndCutsceneTransition()
    {
        // Disable the cutscene camera
        if (_disableOnEndCutscene != null)
        {
            _disableOnEndCutscene.SetActive(false);
        }
        // Wait for a short duration before executing the transition to allow for cinemachine blending
        yield return new WaitForSeconds(_delay);

        // Load the game scene after the cutscene ends
        PlayerInstance.Instance.transform.GetChild(10).GetComponent<Canvas>().enabled = true; // Show the player UI
        PlayerInstance.Instance.transform.GetChild(10).GetComponent<FadeElementInOut>().FadeIn(); // Fade in the player UI
        // InputReader.Instance.SwitchInputMap(InputMap.Player); // Switch to player input map
        GameState.Instance.UnloadCutsceneEndTransitionScene();
    }

}
