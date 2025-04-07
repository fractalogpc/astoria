using UnityEngine;
using System.Collections;

public class SpiderWalking : MonoBehaviour
{

    [SerializeField] private Transform[] _footTargets;
    [SerializeField] private Transform[] _footTargetTrackers;

    [SerializeField] private Transform _body;
    [SerializeField] private Transform _bodyTargetTracker;
    [SerializeField] private Vector3 _bodyPositionOffset = new Vector3(0, 0.5f, 0);

    [SerializeField] private float _stepDistance = 0.5f;
    [SerializeField] private float _stepHeight = 0.2f;
    [SerializeField] private float _stepSpeed = 1f;
    [SerializeField] private LayerMask _groundLayerMask;

    [SerializeField] private Transform _targetTransform;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private AnimationCurve _stepCurve;
    [SerializeField] private float _stepCooldownMean = 0.5f;
    [SerializeField] private float _stepCooldownVariation = 0.2f;
    [SerializeField] private float _tiltIntensity = 10f;
    [SerializeField] private float _pitchIntensity = 10f;

    private Vector3[] _footPositions;
    private bool[] _isStepping;
    private bool[] _hasSteppedThisCycle;
    private bool _allowingZeroIndex = true;

    private void Awake() {
        _footPositions = new Vector3[_footTargets.Length];
        _isStepping = new bool[_footTargets.Length];
        _hasSteppedThisCycle = new bool[_footTargets.Length];
        for (int i = 0; i < _footTargets.Length; i++)
        {
            _footPositions[i] = _footTargets[i].position;
            _footTargetTrackers[i].position = _footTargets[i].position;
            _isStepping[i] = false;
            _hasSteppedThisCycle[i] = false;
        }
    }

    private void Update() {

        // Move body towards target transform
        if (_targetTransform != null) {
            Vector3 targetPosition = _targetTransform.position;
            Vector3 direction = (targetPosition - _bodyTargetTracker.position).normalized;
            direction.y = 0; // Only move in the x-z plane because the trackers find height already
            _bodyTargetTracker.position += direction * _moveSpeed * Time.deltaTime;
        }

        // Update body height and rotation
        Vector3 averagePos = Vector3.zero;
        for (int i = 0; i < _footPositions.Length; i++)
        {
            averagePos += _footPositions[i];
        }
        averagePos /= _footPositions.Length;
        _body.position = averagePos + _bodyPositionOffset;
        // Rotate body tilt and pitch based on foot heights
        float avgHeightRight = 0f;
        float avgHeightLeft = 0f;
        for (int i = 0; i < _footPositions.Length; i++)
        {
            if (i < 4) // Left foot
            {
                avgHeightLeft += _footPositions[i].y;
            }
            else // Right foot
            {
                avgHeightRight += _footPositions[i].y;
            }
        }
        avgHeightLeft /= 4f;
        avgHeightRight /= 4f;
        float tilt = avgHeightRight - avgHeightLeft;

        float avgHeightFront = 0f;
        float avgHeightBack = 0f;
        bool[] frontPositions = new bool[8] { true, true, false, false, true, true, false, false};
        for (int i = 0; i < _footPositions.Length; i++)
        {
            if (frontPositions[i]) // Front foot
            {
                avgHeightFront += _footPositions[i].y;
            }
            else // Back foot
            {
                avgHeightBack += _footPositions[i].y;
            }
        }
        avgHeightFront /= 4f;
        avgHeightBack /= 4f;
        float pitch = avgHeightBack - avgHeightFront;

        _body.rotation = Quaternion.Euler(pitch * _pitchIntensity, _body.rotation.eulerAngles.y, tilt * _tiltIntensity); // Rotate body based on foot heights

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
        if (_hasSteppedThisCycle[footIndex]) return; // Already stepped this cycle

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
            // When excepttion is met, switch to the other index
            _allowingZeroIndex = !_allowingZeroIndex; // Toggle allowing zero index for zigzag pattern
            for (int i = 0; i < _hasSteppedThisCycle.Length; i++)
            {
                _hasSteppedThisCycle[i] = false; // Reset stepped this cycle for all feet
            }
        }

        _isStepping[footIndex] = true;
        _hasSteppedThisCycle[footIndex] = true; // Mark as stepped this cycle
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
            float curveValue = _stepCurve.Evaluate(t);
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, curveValue) + new Vector3(0, height, 0);
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
            if (!(footIndex % 2 == 0 && !_allowingZeroIndex) || (footIndex % 2 != 0 && _allowingZeroIndex)) {
                _isStepping[footIndex] = true; // Prevent exception from being triggered
                // Randomize step cooldown
                float stepCooldown = _stepCooldownMean + Random.Range(-_stepCooldownVariation, _stepCooldownVariation);
                yield return new WaitForSeconds(stepCooldown);

                for (int i = 0; i < _hasSteppedThisCycle.Length; i++)
                {
                    _hasSteppedThisCycle[i] = false; // Reset stepped this cycle for all feet
                }
                _allowingZeroIndex = !_allowingZeroIndex; // Toggle allowing zero index for zigzag pattern
                _isStepping[footIndex] = false; // Allow stepping again
            }
        }
    }
    
}
