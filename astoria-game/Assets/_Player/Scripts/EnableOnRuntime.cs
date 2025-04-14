using UnityEngine;

public class EnableOnRuntime : MonoBehaviour
{

    private void Awake()
    {
        gameObject.SetActive(true);
    }

}
