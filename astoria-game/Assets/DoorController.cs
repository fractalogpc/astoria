using UnityEngine;

public class DoorController : MonoBehaviour
{
    private bool isOpen = false;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        Debug.Log("Open Door");
        if (!isOpen)
        {
            _animator.SetTrigger(GetOpenDirection());
            isOpen = true;
        }
        else
        {
            _animator.SetTrigger("Close");
            isOpen = false;
        }
    }

    private string GetOpenDirection()
    {
        Vector3 playerPosition = PlayerInstance.Instance.transform.position;
        Vector3 doorPosition = transform.position;

        Vector3 direction = (playerPosition - doorPosition).normalized;
        float angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);

        if (angle >= -90 && angle <= 90)
        {
            return "OpenBackwards";
        }
        else
        {
            return "OpenForwards";
        }
    }
}
