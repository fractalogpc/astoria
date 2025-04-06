using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PatrolEnemy : MonoBehaviour
{

    public UnityEvent OnAttack;
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private float _patrolSpeed = 2f;
    [SerializeField] private float _patrolWaitTime = 2f;
    [SerializeField] private float _speedVariation = 0.5f;
    [SerializeField] private float _timeVariation = 0.5f;
    [SerializeField] private float _strayDistance = 1f;
    [SerializeField] private float _maxChaseDistance = 5f;
    [SerializeField] private float _chaseSpeed = 4f;
    [SerializeField] private float _playerDetectionDistance = 10f;
    [SerializeField] private float _attackDistance = 1f;
    [SerializeField] private float _attackDamage = 1f;
    [SerializeField] private float _attackSpeed = 1f;

    [SerializeField] private Transform _player;
    private int _currentPatrolIndex = 0;
    private float _waitTime;
    private NavMeshAgent _navMeshAgent;
    private bool _isPatrolling = true;
    private float _attackTimer = 0f;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _patrolSpeed + Random.Range(-_speedVariation, _speedVariation);
        _waitTime = Random.Range(_patrolWaitTime - _timeVariation, _patrolWaitTime + _timeVariation);
    }

    public void SetPlayer(Transform player)
    {
        _player = player;
    }

    private void Update()
    {
        _attackTimer += Time.deltaTime;
        if (_player == null) {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        if (distanceToPlayer <= _playerDetectionDistance)
        {
            //Debug.Log("Player detected!");
            ChasePlayer();
            _isPatrolling = false;
        }
        else
        {
            //Debug.Log("Player not detected!");
            Patrol();
            _isPatrolling = true;
        }
    }

    private void Patrol()
    {
        if (_patrolPoints.Length == 0) return;

        if (!_isPatrolling)
        {
            _navMeshAgent.speed = _patrolSpeed + Random.Range(-_speedVariation, _speedVariation);
            _navMeshAgent.SetDestination(_patrolPoints[_currentPatrolIndex].position);
        }

        Transform targetPoint = _patrolPoints[_currentPatrolIndex];
        Vector2 _targetPoint = new Vector2(targetPoint.position.x, targetPoint.position.z);
        Vector2 _currentPosition = new Vector2(transform.position.x, transform.position.z);
        float distanceToTarget = Vector2.Distance(_targetPoint, _currentPosition);

        if (distanceToTarget <= _strayDistance)
        {
            _waitTime -= Time.deltaTime;
            if (_waitTime <= 0f)
            {
                _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Length;
                _navMeshAgent.speed = _patrolSpeed + Random.Range(-_speedVariation, _speedVariation);
                _waitTime = Random.Range(_patrolWaitTime - _timeVariation, _patrolWaitTime + _timeVariation);
                _navMeshAgent.SetDestination(targetPoint.position);
            }
        }
    }

    private void ChasePlayer()
    {
        _navMeshAgent.speed = _chaseSpeed;
        _navMeshAgent.SetDestination(_player.position);

        float distanceFromPatrolPoint = Vector3.Distance(transform.position, _patrolPoints[_currentPatrolIndex].position);
        if (distanceFromPatrolPoint > _maxChaseDistance)
        {
            _navMeshAgent.SetDestination(_patrolPoints[_currentPatrolIndex].position);
            _navMeshAgent.speed = _patrolSpeed + Random.Range(-_speedVariation, _speedVariation);
            _isPatrolling = true;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        if (distanceToPlayer < _attackDistance && _attackTimer >= _attackSpeed)
        {
            _attackTimer = 0f;

            // Attack the player
            OnAttack?.Invoke();
            DamageTarget(_player.gameObject, _attackDamage, _player.position);
        }
    }

    private void DamageTarget(GameObject target, float damage, Vector3 position)
    {
        // See if the target has a player health component
        target.TryGetComponent<HealthManager>(out HealthManager health);
        if (health != null)
        {
            health.TakeDamage(damage, position);
        }
    }        

}
