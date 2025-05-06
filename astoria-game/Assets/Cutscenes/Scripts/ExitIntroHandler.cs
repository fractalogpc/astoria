using UnityEngine;
using UnityEngine.Serialization;

public class ExitIntroHandler : MonoBehaviour
{

    [FormerlySerializedAs("destroyOnTriggerEnter")] [SerializeField] private GameObject[] destroyOnExit;

    public void ExitIntro() {
        GameState.Instance.EndCutsceneTriggered();
        foreach (GameObject obj in destroyOnExit)
        {
            Destroy(obj);
        }
        Destroy(gameObject);
    }
    
    private void Update()
    {
        bool overlapped = false;
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                overlapped = true;
                break;
            }
        }
        if (overlapped)
        {
            Debug.Log("Player entered exit cutscene trigger, skipping intro");
            ExitIntro();
        }
    }
    
}
