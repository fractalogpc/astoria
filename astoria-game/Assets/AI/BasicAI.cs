using UnityEngine;

public class BasicAI : MonoBehaviour
{
    public void Death()
    {
        Destroy(this.gameObject);
    }
}
