using UnityEngine;

public class PlayerTriggerManager : MonoBehaviour
{

    [System.Serializable] public class TriggerEvent : UnityEngine.Events.UnityEvent { }
    
    [SerializeField] private TriggerEvent _onTriggerEnter;
    [SerializeField] private TriggerEvent _onTriggerExit;

    private void OnTriggerEnter(Collider other) {
        _onTriggerEnter.Invoke();
    }

    private void OnTriggerExit(Collider other) {
        _onTriggerExit.Invoke();
    }

}
