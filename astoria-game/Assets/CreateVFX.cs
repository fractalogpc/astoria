using Unity.Mathematics;
using UnityEngine;

public class CreateVFX : MonoBehaviour
{

    public GameObject effectPrefab;

    public void CreateEffect(Vector3 position)
    {
        GameObject effect = Instantiate(effectPrefab, position, quaternion.identity);
        Destroy(effect, 2f);
    }

    public void CreateEffect(GameObject effectPrefab, Vector3 position, Quaternion rotation, Vector3 scale, float duration)
    {
        GameObject effect = Instantiate(effectPrefab, position, rotation);
        effect.transform.localScale = scale;
        Destroy(effect, duration);
    }
}
