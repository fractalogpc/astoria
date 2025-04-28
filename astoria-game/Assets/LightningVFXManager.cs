using UnityEngine;
using UnityEngine.VFX;
using FMODUnity;

public class LightningVFXManager : MonoBehaviour
{

    [SerializeField] private Transform playerTransform;
    [SerializeField] private EventReference thunderEvent;

    [SerializeField] private Vector2 lightningDelayRange = new Vector2(0.5f, 1.5f);
    [SerializeField] private Vector2 lightningRadius;
    [SerializeField] private LayerMask lightningStrikeLayerMask;

    private VisualEffect lightningVFX;
    private VFXEventAttribute lightningPositionAttribute;

    private float timeUntilNextLightning;

    private void Awake()
    {
        timeUntilNextLightning = Random.Range(lightningDelayRange.x, lightningDelayRange.y);
    }

    void Start()
    {
        lightningVFX = GetComponent<VisualEffect>();
        if (lightningVFX == null)
        {
            Debug.LogError("Lightning VFX component not found on this GameObject.");
            return;
        }

        lightningPositionAttribute = lightningVFX.CreateVFXEventAttribute();
    }

    void Update()
    {
        timeUntilNextLightning -= Time.deltaTime;

        if (timeUntilNextLightning <= 0f)
        {
            // Randomize the delay for the next lightning strike
            timeUntilNextLightning = Random.Range(lightningDelayRange.x, lightningDelayRange.y);

            // Randomize the position of the lightning strike within a radius around the player
            float radius = Random.Range(lightningRadius.x, lightningRadius.y);
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection.y = 0; // Keep it horizontal
            Vector3 lightningPosition = playerTransform.position + randomDirection;

            RaycastHit hit;
            if (Physics.Raycast(lightningPosition + Vector3.up * 100f, Vector3.down, out hit, Mathf.Infinity, lightningStrikeLayerMask))
            {
                SpawnLightning(hit.point);
            }
            else
            {
                Debug.LogWarning("No valid surface found for lightning strike.");
            }
        }
    }

    private void SpawnLightning(Vector3 position)
    {
        // Set the position of the lightning effect to the player's position
        lightningPositionAttribute.SetVector3("targetPosition", position);
        lightningVFX.SendEvent("SpawnLightning", lightningPositionAttribute);

        // Play the thunder sound effect
        RuntimeManager.PlayOneShot(thunderEvent, position);
    }

}
