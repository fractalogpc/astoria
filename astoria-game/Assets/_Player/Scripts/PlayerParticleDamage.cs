using UnityEngine;

public class PlayerParticleDamage : MonoBehaviour
{

    [SerializeField] private float damageAmount = 10f; // Amount of damage to apply
    [SerializeField] private float damageInterval = 1f; // Minimum time interval between damage application
    
    public GameObject player; // Reference to the player object
    private float timeSinceLastDamage; // Time when the last damage was applied
    
    private void OnParticleTrigger() {
        // Damage player
        if (timeSinceLastDamage >= damageInterval) {
            // Assuming the player has a method to apply damage
            player.GetComponent<HealthManager>().Damage(damageAmount, transform.position);
            timeSinceLastDamage = 0f; // Reset the timer
        }
    }

    private void Update() {
        timeSinceLastDamage += Time.deltaTime;
    }

}
