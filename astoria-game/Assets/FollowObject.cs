using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private Vector3 offset;

    private void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.rotation = target.rotation;
        }
    }
}
