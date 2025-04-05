using UnityEngine;
using UnityEngine.Events;

public class FinalMonumentController : MonoBehaviour
{

    public UnityEvent OnMonumentEnabled;

    private int numberOfMonumentsActivated = 0;

    public void AddMonument() {
        numberOfMonumentsActivated++;

        if (numberOfMonumentsActivated >= 3) {
            OnMonumentEnabled?.Invoke();
        }
    }
}
