using UnityEngine;
using UnityEngine.AI;

public class GroundMeleeEnemy : EnemyCore
{

  [Header("Attack Settings")]
  [SerializeField] private float attackRadius;
  [SerializeField] private float attackDamage;
  [SerializeField] private float attackSpeed;

  [Header("Targeting Settings")]
  [SerializeField] private float playerFocusRadius;
  [SerializeField] private float obstacleSphereCastRadius;
  [SerializeField] private LayerMask obstacleMask;
  [SerializeField] private float obstacleNoticeDistance;

  private NavMeshAgent agent;
  private Transform target;
  private float attackTimer;
  // private Animator animator;

  private void Start() {
    // Animator needs getting
  }

  public override void Navigate(Transform core, Transform player) {
    // Pick between player and core based on player proximity
    float playerDistance = Vector3.Distance(player.position, transform.position);
    Transform goal = core;
    if (playerDistance < playerFocusRadius) goal = player;

    // Find any obstacles between the enemy and the goal
    RaycastHit hit;
    if (Physics.SphereCast(transform.position, obstacleSphereCastRadius, goal.position - transform.position, out hit, obstacleNoticeDistance, obstacleMask)) {
      // If there is an obstacle, navigate to it and destroy it
      agent.SetDestination(hit.point);
      target = hit.transform;
      return;
    }

    agent.SetDestination(goal.position);
  }

  public override void Attack() {
    attackTimer += Time.deltaTime;
    if (attackTimer < attackSpeed) {
      return;
    }
    // Find if the target is in the attack radius
    if (Vector3.Distance(transform.position, target.position) <= attackRadius) {
      // Attack the target
      // animator.SetTrigger("Attack"); // Play attack animation
      attackTimer = 0;
      base.DamageTarget(target.gameObject, attackDamage, target.position);
    }
  }

}