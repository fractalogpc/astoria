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
  
  [Header("Targeting Settings")]
  [SerializeField] private float playerFocusRadius;
  [SerializeField] private float obstacleSphereCastRadius;
  [SerializeField] private LayerMask obstacleMask;
  [SerializeField] private float obstacleNoticeDistance;

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

  public override void Navigate(Transform player) {
    RaycastHit hit;
    if (Physics.SphereCast(transform.position, obstacleSphereCastRadius, player.position - transform.position, out hit, obstacleNoticeDistance, obstacleMask)) {
      // If there is an obstacle, navigate to it and destroy it
      agent.SetDestination(hit.point);
      target = hit.transform;
      return;
    } else {
      target = player;
    }

    agent.SetDestination(player.position);
  }

  public override void Attack() {
    attackTimer += Time.deltaTime;
    if (attackTimer < attackSpeed) {
      return;
    }

    if (Vector3.Distance(transform.position, target.position) < 10 && target) {
      attackTimer = 0;
      transform.LookAt(target, Vector3.up);

      GameObject projectile = Instantiate(projectilePrefab);
      projectile.GetComponent<Rigidbody>().AddForce(transform.forward * propulsionFactor);
    }
  }

}