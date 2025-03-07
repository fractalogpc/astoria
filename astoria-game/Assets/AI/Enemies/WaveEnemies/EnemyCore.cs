using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Construction;

public abstract class EnemyCore : MonoBehaviour
{

  private Transform _core;
  private Transform _player;

  private void Update() {
    Navigate(_core, _player);
    Attack();
  }

  protected void DamageTarget(GameObject target, float damage, Vector3 position) {
    // See if the target has a player or building health component
    target.TryGetComponent<HealthInterface>(out HealthInterface health);
    if (health != null) {
      health.TakeDamage(damage, position);
    }

    target.TryGetComponent<ConstructionComponent>(out ConstructionComponent building);
    if (building != null) {
      building.Damage(damage);
    }
  }

  public abstract void Navigate(Transform core, Transform player);

  public abstract void Attack();
  
}