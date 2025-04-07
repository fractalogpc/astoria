using UnityEngine;

public class PlayerParticleDamage : MonoBehaviour
{

    [SerializeField] private float damageAmount = 10f; // Amount of damage to apply
    [SerializeField] private float damageInterval = 1f; // Minimum time interval between damage application
    
    public GameObject player; // Reference to the player object
    private float timeSinceLastDamage; // Time when the last damage was applied
    
    private void Start() {
        // Initialize the time since last damage to the damage interval to avoid immediate damage
        timeSinceLastDamage = damageInterval;
    }

    private void OnParticleTrigger() {
        // Damage player
        // Debug.Log("PlayerParticleDamage: OnParticleTrigger called");
        if (timeSinceLastDamage >= damageInterval) {
            // Debug.Log("PlayerParticleDamage: Damaging player");
            player.GetComponent<HealthManager>().Damage(damageAmount, transform.position);
            timeSinceLastDamage = 0f; // Reset the timer
        }
    }

    private void Update() {
        timeSinceLastDamage += Time.deltaTime;
    }

}
