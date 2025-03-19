using UnityEngine;
using UnityEngine.AI;

public class GroundMeleeAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private GroundMeleeEnemy _core;
    [SerializeField] private float _runThreshold = 1.0f;
    private void Update() {
        _animator.SetFloat("MovementSpeed", _agent.velocity.magnitude);
        _animator.SetBool("Running", _agent.velocity.magnitude >= _runThreshold);
        _animator.SetBool("Attack", _core._attacking);
    }
}
