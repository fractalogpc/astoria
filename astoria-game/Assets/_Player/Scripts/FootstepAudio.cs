using FMODUnity;
using Player;
using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float _footstepDistance = 1f;
    
    private EventReference _footstepsEvent;
    private Vector3 _lastFootstepPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _footstepsEvent = FMODEvents.Instance.FootstepsEvent;
        _lastFootstepPosition = transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (_playerController.MoveInputVector.magnitude < 0.1f) return;
        Vector3 positionXZ = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 lastPositionXZ = new Vector3(_lastFootstepPosition.x, 0, _lastFootstepPosition.z);
        print(Vector3.Distance(positionXZ, lastPositionXZ));
        if (!(Vector3.Distance(positionXZ, lastPositionXZ) > _footstepDistance)) return;
        AudioManager.Instance.PlayOneShot(_footstepsEvent, transform.position);
        _lastFootstepPosition = transform.position;
    }
}
