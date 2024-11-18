using UnityEngine;
using UnityEngine.AI;

public class MoveToTarget : MonoBehaviour
{
  
  [SerializeField] private Transform _target;

  private NavMeshAgent _agent;

  private void Start() {
    _target = GameObject.FindWithTag("Player").transform;
    _agent = GetComponent<NavMeshAgent>();
  }

  private void Update() {
    _agent.SetDestination(_target.position);
  }

}
