using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Construction;

public abstract class EnemyCore : MonoBehaviour
{

  private Transform _player;

  private void Update() {
    Navigate(_player);
    Attack();
  }

  public void SetPlayer(Transform player) {
    _player = player;
  }

  protected void DamageTarget(GameObject target, float damage, Vector3 position) {
    // See if the target has a player or building health component
    target.TryGetComponent<HealthManager>(out HealthManager health);
    if (health != null) {
      health.TakeDamage(damage, position);
    }

    target.TryGetComponent<ConstructionComponent>(out ConstructionComponent building);
    if (building != null) {
      building.TakeDamage(damage, position);
    }

    target.TryGetComponent<CoreController>(out CoreController core);
    if (core != null) {
      core.TakeDamage(damage, position);
    }
  }

  public abstract void Navigate(Transform player);

  public abstract void Attack();
  
}