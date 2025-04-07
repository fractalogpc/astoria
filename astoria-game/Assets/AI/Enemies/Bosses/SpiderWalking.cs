using UnityEngine;
using System.Collections;

public class SpiderWalking : MonoBehaviour
{

    [SerializeField] private Transform[] _footTargets;
    [SerializeField] private Transform[] _footTargetTrackers;

    [SerializeField] private Transform _body;

    [SerializeField] private float _stepDistance = 0.5f;
    [SerializeField] private float _stepHeight = 0.2f;
    [SerializeField] private float _stepSpeed = 1f;
    [SerializeField] private LayerMask _groundLayerMask;

    [SerializeField] private Transform _targetTransform;
    [SerializeField] private float _moveSpeed = 1f;

    private Vector3[] _footPositions;
    private bool[] _isStepping;
    private bool _allowingZeroIndex = true;

    private void Awake() {
        _footPositions = new Vector3[_footTargets.Length];
        _isStepping = new bool[_footTargets.Length];
        for (int i = 0; i < _footTargets.Length; i++)
        {
            _footPositions[i] = _footTargets[i].position;
            _footTargetTrackers[i].position = _footTargets[i].position;
            _isStepping[i] = false;
        }
    }

    private void Update() {

        // Move body towards target transform
        if (_targetTransform != null) {
            Vector3 targetPosition = _targetTransform.position;
            Vector3 direction = (targetPosition - _body.position).normalized;
            _body.position += direction * _moveSpeed * Time.deltaTime;
        }

        for (int i = 0; i < _footTargets.Length; i++)
        {
            _footTargets[i].position = _footPositions[i];
        }

        for (int i = 0; i < _footTargetTrackers.Length; i++)
        {
            Vector3 footTargetPosition = _footPositions[i];

            // Snap foot target tracker to ground
            RaycastHit hit;
            if (Physics.Raycast(_footTargetTrackers[i].position + Vector3.up * 50.0f, Vector3.down, out hit, Mathf.Infinity, _groundLayerMask))
            {
                _footTargetTrackers[i].position = new Vector3(_footTargetTrackers[i].position.x, hit.point.y, _footTargetTrackers[i].position.z);
            }
            Vector3 footTargetTrackerPosition = _footTargetTrackers[i].position;
            float distance = Vector3.Distance(footTargetPosition, footTargetTrackerPosition);
            if (distance >= _stepDistance)
            {  
                Vector3 direction = (footTargetTrackerPosition - footTargetPosition).normalized;
                Vector3 targetPosition = footTargetPosition + direction * _stepDistance;
                // Take a step
                TakeStep(i, targetPosition, footTargetPosition);
            }
        }
    }

    private void TakeStep(int footIndex, Vector3 footTargetTrackerPosition, Vector3 footTargetPosition) {
        if (_isStepping[footIndex]) return; // Already stepping

        // Only allow step if accomodates zigzag pattern
        if ((footIndex % 2 == 0 && !_allowingZeroIndex) || (footIndex % 2 == 1 && _allowingZeroIndex)) {
            // Exception if nothing else is stepping
            bool anyStepping = false;
            for (int i = 0; i < _isStepping.Length; i++)
            {
                if (_isStepping[i])
                {
                    anyStepping = true;
                    break;
                }
            }
            if (anyStepping) return; // Skip even index if not allowing zero index
        }

        _isStepping[footIndex] = true;
        StartCoroutine(StepCoroutine(footIndex, footTargetTrackerPosition, footTargetPosition));
    }

    private IEnumerator StepCoroutine(int footIndex, Vector3 footTargetTrackerPosition, Vector3 footTargetPosition) {
        Vector3 startPosition = _footPositions[footIndex];
        Vector3 endPosition = footTargetTrackerPosition;
        float elapsedTime = 0f;

        while (elapsedTime < _stepSpeed) {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _stepSpeed);
            float height = Mathf.Sin(t * Mathf.PI) * _stepHeight;
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, t) + new Vector3(0, height, 0);
            _footPositions[footIndex] = newPosition;
            yield return null;
        }

        _footPositions[footIndex] = footTargetTrackerPosition;
        _isStepping[footIndex] = false;

        bool allFeetStepped = true;
        for (int i = 0; i < _footTargets.Length; i++)
        {
            if (_isStepping[i])
            {
                allFeetStepped = false;
                break;
            }
        }

        if (allFeetStepped)
        {
            // Only toggle zero index if this foot belongs to the allowed zero index
            if (!(footIndex % 2 == 0 && !_allowingZeroIndex) || (footIndex % 2 != 0 && _allowingZeroIndex)){
                _allowingZeroIndex = !_allowingZeroIndex; // Toggle allowing zero index for zigzag pattern
            }
        }
    }
    
}
