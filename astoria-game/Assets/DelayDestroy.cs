using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    public void Destroy(float delay = 0.05f)
    {
        Destroy(gameObject, delay);
    }
}
