using Mirror.Examples.Common;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// THIS SCRIPT WAS MADE DAY BEFORE OF OGPC
/// Don't use it anywhere
/// </summary>
public class OnLateStart : MonoBehaviour
{
    public float delay;
    public UnityEvent OnStart;

    public PlayerCamera playerCamera;

    private void Start()
    {
        InputReader.Instance.SwitchInputMap(InputMap.Null);
        // playerCamera = FindFirstObjectByType<PlayerCamera>();
        // if (playerCamera != null)
        // {
        //     playerCamera.GetComponent<PlayerCamera>().enabled = false;
        //     print("yay");
        // }
        Invoke(nameof(ExecuteOnStart), delay);
        Invoke(nameof(SwitchToPlayerMap), delay);
    }
    private void ExecuteOnStart()
    {
        OnStart?.Invoke();
    }
    private void SwitchToPlayerMap()
    {
        // playerCamera.enabled = true;
        InputReader.Instance.SwitchInputMap(InputMap.Player);
    }
}
