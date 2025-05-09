using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class GroundMeleeEnemy : EnemyCore
{
  public UnityEvent OnAttack;
  
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
  public bool _attacking;
  // private Animator animator;

  private void Start() {
    // Animator needs getting
    agent = GetComponent<NavMeshAgent>();
  }

  public override void Navigate(Transform player) {
    // Pick between player and core based on player proximity
    float playerDistance = Vector3.Distance(player.position, transform.position);
    target = player;
    NavMeshPath path = new NavMeshPath();
    if (!agent.CalculatePath(player.position, path)) {
      RaycastHit hit;
      if (Physics.SphereCast(transform.position, obstacleSphereCastRadius, player.position - transform.position, out hit, obstacleNoticeDistance, obstacleMask)) {
        // If there is an obstacle, navigate to it and destroy 
        agent.SetDestination(hit.point);
        target = hit.transform;
        return;
      } else {
        agent.SetDestination(player.position);
      }
    } else {
      agent.SetDestination(player.position);
    }
  }

  public override void Attack() {
    if (target == null) return;
    attackTimer += Time.deltaTime;
    if (attackTimer < attackSpeed) {
      return;
    }
    // Find if the target is in the attack radius
    if (Vector3.Distance(transform.position, target.position) <= attackRadius) {
      // Attack the target
      // animator.SetTrigger("Attack"); // Play attack animation
      Debug.Log("Attacking: " + target.name);
      _attacking = true;
      attackTimer = 0;
      OnAttack?.Invoke();
      target.gameObject.GetComponentInChildren<IDamageable>().TakeDamage(attackDamage, target.position);
      _attacking = false;
    }
  }

  void OnCollisionEnter(Collision collision) {
    if (collision.gameObject.layer == 18) {
      target = collision.transform;
    }
  }

}