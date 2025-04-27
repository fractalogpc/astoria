using UnityEngine;
using UnityEngine.VFX;

public class RainUpdater : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private VisualEffect rainVFX;

    void Update()
    {
        rainVFX.SetVector3("PlayerPosition", playerTransform.position);
    }


    public void IncreaseRain()
    {
        rainVFX.SetFloat("Alpha", 0.01f);
        rainVFX.SetFloat("AlphaCameraThreshold", 1f);
    }
}
