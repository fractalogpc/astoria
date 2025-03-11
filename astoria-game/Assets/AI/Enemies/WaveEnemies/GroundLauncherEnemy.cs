using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GroundLauncherEnemy : EnemyCore
{

  [Header("Attack Settings")]
  [SerializeField] private float firingRadius;
  [SerializeField] private float attackSpeed;
  [SerializeField] private float propulsionFactor;
  [SerializeField] private GameObject projectilePrefab;

  private NavMeshAgent agent;
  private Transform target;
  private float attackTimer;
  private Vector3 firingLocation = Vector3.zero;

  private void Start() {
    agent = GetComponent<NavMeshAgent>();

    Vector2 firingLocationV2 = Random.insideUnitCircle.normalized * firingRadius;
    RaycastHit hit;
    if (Physics.Raycast(new Vector3(firingLocationV2.x, 10000, firingLocationV2.y), Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground"))) {
      float y = 10000 - hit.distance;
      firingLocation = new Vector3(firingLocationV2.x, y, firingLocationV2.y);
    }
  }

  public override void Navigate(Transform core, Transform player) {
    target = core;
    agent.SetDestination(firingLocation);
  }

  public override void Attack() {
    attackTimer += Time.deltaTime;
    if (attackTimer < attackSpeed) {
      return;
    }

    if (Vector3.Distance(transform.position, firingLocation) < 3 && target) {
      attackTimer = 0;
      transform.LookAt(target, Vector3.up);
      GameObject projectile = Instantiate(projectilePrefab);

      projectile.GetComponent<Rigidbody>().AddForce(transform.forward * propulsionFactor);
    }
  }

}