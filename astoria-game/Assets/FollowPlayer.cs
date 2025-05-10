using UnityEngine;
using KinematicCharacterController;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private KinematicCharacterMotor target;

    [SerializeField] private Vector3 offset;

    private void Update()
    {
        if (target != null)
        {
            transform.position = target.InitialSimulationPosition + offset;
            transform.rotation = target.InitialSimulationRotation;
        }
    }
}
