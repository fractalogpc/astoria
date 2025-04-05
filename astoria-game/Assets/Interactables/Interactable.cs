using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
  public UnityEvent OnInteract;
  public virtual void Interact() {
    OnInteract?.Invoke();

    if (OnInteract.GetPersistentEventCount() == 0) {
      // Debug.Log("Interacted");
    }
  }
}
