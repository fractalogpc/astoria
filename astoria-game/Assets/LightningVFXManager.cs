using UnityEngine;
using UnityEngine.VFX;

public class LightningVFXManager : MonoBehaviour
{

    [SerializeField] private Transform playerTransform;

    private VisualEffect lightningVFX;

    void Start()
    {
        lightningVFX = GetComponent<VisualEffect>();
        if (lightningVFX == null)
        {
            Debug.LogError("Lightning VFX component not found on this GameObject.");
            return;
        }

        // Set the initial position of the lightning effect to the player's position
        lightningVFX.SetVector3("CenterPosition", playerTransform.position);
    }

    void Update()
    {
        if (lightningVFX != null)
        {
            // Update the position of the lightning effect to follow the player
            lightningVFX.SetVector3("CenterPosition", playerTransform.position);
        }
    }

}
