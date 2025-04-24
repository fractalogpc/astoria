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
        GameState.Instance.UnloadCutsceneEndTransitionScene();
    }

}
