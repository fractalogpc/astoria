using System;
using UnityEngine;
using UnityEngine.AI;

public class WandererMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Vector3? _target;
    
    public void SetTarget(Transform target) {
        _target = target.position;
        _agent.SetDestination(_target.Value);
    }
    public void SetTarget(Vector3 target) {
        _target = target;
        _agent.SetDestination(_target.Value);
    }
    public bool Go() {
        if (_target == null) return false;
        _agent.isStopped = false;
        _agent.SetDestination(_target.Value);
        return true;
    }
    public void Stop() {
        _agent.isStopped = true;
    }
}