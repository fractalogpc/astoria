using UnityEngine;

public class Health : MonoBehaviour
{

  [SerializeField] private int _maxHealth = 100;
  [SerializeField] private int _currentHealth;

  private void Start() {
    _currentHealth = _maxHealth;
  }

  public void TakeDamage(int damage) {
    _currentHealth -= damage;
    if (_currentHealth <= 0) {
      Die();
    }
  }

  private void Die() {
    Destroy(gameObject);
  }

}
