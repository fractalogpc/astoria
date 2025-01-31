using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "NavigateInterruptable", story: "[Self] navigates to [player] and sets [state]", category: "Action/Navigation", id: "5a6abd6203d883e017c08a6f0a1d63e1")]
public partial class NavigateInterruptableAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<State> m_State;
    
    [SerializeReference] public BlackboardVariable<float> Speed = new BlackboardVariable<float>(1.0f);
    [SerializeReference] public BlackboardVariable<float> DistanceThreshold = new BlackboardVariable<float>(0.2f);
    [SerializeReference] public BlackboardVariable<string> AnimatorSpeedParam = new BlackboardVariable<string>("SpeedMagnitude");

    // This will only be used in movement without a navigation agent.
    [SerializeReference] public BlackboardVariable<float> SlowDownDistance = new BlackboardVariable<float>(1.0f);

    private NavMeshAgent m_NavMeshAgent;
    private Animator m_Animator;
    private float m_PreviousStoppingDistance;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_ColliderAdjustedTargetPosition;
    private float m_ColliderOffset;

    protected override Status OnStart()
    {
        if (Self.Value == null || Player.Value == null)
        {
            return Status.Failure;
        }

        return Initialize();
    }

    protected override Status OnUpdate()
    {
        if (Self.Value == null || Player.Value == null)
        {
            return Status.Failure;
        }

        // Check if the target position has changed.
        bool boolUpdateTargetPosition = !Mathf.Approximately(m_LastTargetPosition.x, Player.Value.transform.position.x) || !Mathf.Approximately(m_LastTargetPosition.y, Player.Value.transform.position.y) || !Mathf.Approximately(m_LastTargetPosition.z, Player.Value.transform.position.z);
        if (boolUpdateTargetPosition)
        {
            m_LastTargetPosition = Player.Value.transform.position;
            m_ColliderAdjustedTargetPosition = GetPositionColliderAdjusted();
        }

        float distance = GetDistanceXZ();
        if (distance <= (DistanceThreshold + m_ColliderOffset))
        {
            return Status.Success;
        }

        if (distance < 3) {
            m_State.Value = State.Attacking;
            return Status.Success;
        } else if (distance > 30) {
            m_State.Value = State.Patrolling;
            return Status.Success;
        }

        if (m_NavMeshAgent != null)
        {
            if (boolUpdateTargetPosition)
            {
                m_NavMeshAgent.SetDestination(m_ColliderAdjustedTargetPosition);
            }
        }
        else
        {
            float speed = Speed;

            if (SlowDownDistance > 0.0f && distance < SlowDownDistance)
            {
                float ratio = distance / SlowDownDistance;
                speed = Mathf.Max(0.1f, Speed * ratio);
            }

            Vector3 agentPosition = Self.Value.transform.position;
            Vector3 toDestination = m_ColliderAdjustedTargetPosition - agentPosition;
            toDestination.y = 0.0f;
            toDestination.Normalize();
            agentPosition += toDestination * (speed * Time.deltaTime);
            Self.Value.transform.position = agentPosition;

            // Look at the target.
            Self.Value.transform.forward = toDestination;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        if (m_Animator != null)
        {
            m_Animator.SetFloat(AnimatorSpeedParam, 0);
        }

        if (m_NavMeshAgent != null)
        {
            if (m_NavMeshAgent.isOnNavMesh)
            {
                m_NavMeshAgent.ResetPath();
            }
            m_NavMeshAgent.stoppingDistance = m_PreviousStoppingDistance;
        }

        m_NavMeshAgent = null;
        m_Animator = null;
    }

    protected override void OnDeserialize()
    {
        Initialize();
    }

    private Status Initialize()
    {
        m_LastTargetPosition = Player.Value.transform.position;
        m_ColliderAdjustedTargetPosition = GetPositionColliderAdjusted();

        // Add the extents of the colliders to the stopping distance.
        m_ColliderOffset = 0.0f;
        Collider agentCollider = Self.Value.GetComponentInChildren<Collider>();
        if (agentCollider != null)
        {
            Vector3 colliderExtents = agentCollider.bounds.extents;
            m_ColliderOffset += Mathf.Max(colliderExtents.x, colliderExtents.z);
        }

        if (GetDistanceXZ() <= (DistanceThreshold + m_ColliderOffset))
        {
            return Status.Success;
        }

        // If using animator, set speed parameter.
        m_Animator = Self.Value.GetComponentInChildren<Animator>();
        if (m_Animator != null)
        {
            m_Animator.SetFloat(AnimatorSpeedParam, Speed);
        }

        // If using a navigation mesh, set target position for navigation mesh agent.
        m_NavMeshAgent = Self.Value.GetComponentInChildren<NavMeshAgent>();
        if (m_NavMeshAgent != null)
        {
            if (m_NavMeshAgent.isOnNavMesh)
            {
                m_NavMeshAgent.ResetPath();
            }
            m_NavMeshAgent.speed = Speed;
            m_PreviousStoppingDistance = m_NavMeshAgent.stoppingDistance;

            m_NavMeshAgent.stoppingDistance = DistanceThreshold + m_ColliderOffset;
            m_NavMeshAgent.SetDestination(m_ColliderAdjustedTargetPosition);
        }

        return Status.Running;
    }


    private Vector3 GetPositionColliderAdjusted()
    {
        Collider targetCollider = Player.Value.GetComponentInChildren<Collider>();
        if (targetCollider != null)
        {
            return targetCollider.ClosestPoint(Self.Value.transform.position);
        }
        return Player.Value.transform.position;
    }

    private float GetDistanceXZ()
    {
        Vector3 agentPosition = new Vector3(Self.Value.transform.position.x, m_ColliderAdjustedTargetPosition.y, Self.Value.transform.position.z);
        return Vector3.Distance(agentPosition, m_ColliderAdjustedTargetPosition);
    }
}