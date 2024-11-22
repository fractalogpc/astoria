using UnityEngine;
using UnityEngine.AI;

public class AIChase : MonoBehaviour
{
    [SerializeField] private Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }
}