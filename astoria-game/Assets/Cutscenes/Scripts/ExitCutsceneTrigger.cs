using UnityEngine;

public class ExitCutsceneTrigger : MonoBehaviour
{

    [SerializeField] private GameObject[] destroyOnTriggerEnter;
    
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
            Debug.Log("Player entered exit cutscene trigger.");
            GameState.Instance.EndCutsceneTriggered();
            foreach (GameObject obj in destroyOnTriggerEnter)
            {
                Destroy(obj);
            }
            Destroy(gameObject);
        }
    }

}
