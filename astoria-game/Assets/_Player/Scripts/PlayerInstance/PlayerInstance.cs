using UnityEngine;

public class PlayerInstance : MonoBehaviour
{
    public static PlayerInstance Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
            Debug.LogWarning("Duplicate PlayerInstance destroyed. Only one instance is allowed.");
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
