using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
  [Header("Interaction Settings")]
  public UnityEvent OnInteract;
  public string InteractText => _interactText;
  [SerializeField] protected string _interactText = "Interact";
  public virtual void Interact() {
    OnInteract?.Invoke();
    if (OnInteract.GetPersistentEventCount() == 0) {
      // Debug.Log("Interacted");
    }
  }
}
