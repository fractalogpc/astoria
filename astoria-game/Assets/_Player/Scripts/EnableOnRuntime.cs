using UnityEngine;
using UnityEngine.Events;

public class EnableOnRuntime : MonoBehaviour
{

    public UnityEvent onStart;

    private void Start()
    {
        onStart?.Invoke();
    }
}
