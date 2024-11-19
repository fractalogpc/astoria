using UnityEngine;

public class Interactable : MonoBehaviour
{
  public UnityEngine.Events.UnityEvent OnInteract;
  public virtual void Interact() {
    OnInteract?.Invoke();

    if (OnInteract.GetPersistentEventCount() == 0) {
      Debug.Log("Interacted");
    }
  }
}
