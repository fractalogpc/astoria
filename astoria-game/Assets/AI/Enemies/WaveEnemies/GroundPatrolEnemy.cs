using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEditor;

public class GroundPatrolEnemy : EnemyCore
{
  public UnityEvent OnAttack;
  
  [Header("Attack Settings")]
  [SerializeField] private float _attackDistance = 1f;
  [SerializeField] private float _attackDamage = 1f;
  [SerializeField] private float _attackSpeed = 1f;

  [Header("Targeting Settings")]
  [SerializeField] private Transform[] _patrolPoints;
  [SerializeField] private float _patrolSpeed = 2f;
  [SerializeField] private float _patrolWaitTime = 2f;
  [SerializeField] private float _speedVariation = 0.5f;
  [SerializeField] private float _timeVariation = 0.5f;
  [SerializeField] private float _strayDistance = 1f;
  [SerializeField] private float _maxChaseDistance = 5f;
  [SerializeField] private float _chaseSpeed = 4f;
  [SerializeField] private float _playerDetectionDistance = 10f;
  [SerializeField] private float obstacleSphereCastRadius;
  [SerializeField] private LayerMask obstacleMask;
  [SerializeField] private LayerMask attackMask;
  [SerializeField] private float obstacleNoticeDistance;

  private NavMeshAgent _navMeshAgent;
  private Transform player;
  private Transform target;
  private float attackTimer;
  public bool _attacking;
  private int _currentPatrolIndex = 0;
  private float _waitTime;
  private bool _isPatrolling = true;
  private float _attackTimer = 0f;
  // private Animator animator;

  private void Start() {
    // Animator needs getting
    _navMeshAgent = GetComponent<NavMeshAgent>();
    _waitTime = Random.Range(_patrolWaitTime - _timeVariation, _patrolWaitTime + _timeVariation);
  }

  public override void Navigate(Transform passedPlayer) {
    player = passedPlayer;
    _attackTimer += Time.deltaTime;
    if (player == null) {
      Debug.Log("player is null :skull:");
      return;
    }
    float distanceToPlayer = Vector3.Distance(transform.position, player.position);
    if (distanceToPlayer <= _playerDetectionDistance) {
      Debug.Log("Player detected!");
      ChasePlayer();
      _isPatrolling = false;
    } else {
      Debug.Log("Player not detected!");
      _navMeshAgent.SetDestination(_patrolPoints[_currentPatrolIndex].position);
      Patrol();
      _isPatrolling = true;
    }
  }

  private void Patrol(){
    target = null;
    if (_patrolPoints.Length == 0) return;

    if (!_isPatrolling) {
      _navMeshAgent.speed = _patrolSpeed + Random.Range(-_speedVariation, _speedVariation);
      _navMeshAgent.SetDestination(_patrolPoints[_currentPatrolIndex].position);
    }

    Transform targetPoint = _patrolPoints[_currentPatrolIndex];
    Vector2 _targetPoint = new Vector2(targetPoint.position.x, targetPoint.position.z);
    Vector2 _currentPosition = new Vector2(transform.position.x, transform.position.z);
    float distanceToTarget = Vector2.Distance(_targetPoint, _currentPosition);
    Debug.Log($"Distance to target: {distanceToTarget}");

    if (distanceToTarget <= _strayDistance) {
      _waitTime -= Time.deltaTime;
      if (_waitTime <= 0f)
      {
        _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Length;
        _navMeshAgent.speed = _patrolSpeed + Random.Range(-_speedVariation, _speedVariation);
        _waitTime = Random.Range(_patrolWaitTime - _timeVariation, _patrolWaitTime + _timeVariation);
        targetPoint = _patrolPoints[_currentPatrolIndex];
        _navMeshAgent.SetDestination(targetPoint.position);
      }
    }
  }

  private void ChasePlayer() {
    target = player;
    _navMeshAgent.speed = _chaseSpeed;
    _navMeshAgent.SetDestination(player.position);

    float distanceFromPatrolPoint = Vector3.Distance(transform.position, _patrolPoints[_currentPatrolIndex].position);
    if (distanceFromPatrolPoint > _maxChaseDistance) {
      _navMeshAgent.SetDestination(_patrolPoints[_currentPatrolIndex].position);
      _navMeshAgent.speed = _patrolSpeed + Random.Range(-_speedVariation, _speedVariation);
      _isPatrolling = true;
    }

    float distanceToPlayer = Vector3.Distance(transform.position, player.position);
    if (distanceToPlayer < _attackDistance && _attackTimer >= _attackSpeed) {
      _attackTimer = 0f;

      // Attack the player
      OnAttack?.Invoke();
      DamageTarget(player.gameObject, _attackDamage, player.position);
    }
  }

  public override void Attack() {
    if (target == null) return;
    attackTimer += Time.deltaTime;
    if (attackTimer < _attackSpeed) {
      return;
    }
    // Find if the target is in the attack radius
    if (Vector3.Distance(transform.position, target.position) <= _attackDistance) {
      RaycastHit hit;
      Physics.SphereCast(transform.position, obstacleSphereCastRadius, target.position - transform.position, out hit, _attackDistance, attackMask);
      if (hit.transform == target.transform) {
        // Attack the target
        // animator.SetTrigger("Attack"); // Play attack animation
        Debug.Log("Attacking: " + target.name);
        _attacking = true;
        attackTimer = 0;
        OnAttack?.Invoke();
        target.gameObject.GetComponentInChildren<IDamageable>().TakeDamage(_attackDamage, target.position);
        _attacking = false;
      }
    }
  }

  void OnCollisionEnter(Collision collision) {
    if (collision.gameObject.layer == 18) {
      target = collision.transform;
    }
  }

  void OnDrawGizmos() 
  {
    Handles.Label(transform.position, _isPatrolling ? "patrolling" : "not");
  }

}