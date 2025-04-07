using UnityEngine;
using static UnityEngine.ParticleSystem;
using System.Collections;

public class SpiderWalking : MonoBehaviour
{

    [SerializeField] private Transform[] _footTargets;
    [SerializeField] private Transform[] _footTargetTrackers;

    [SerializeField] private Transform _body;
    [SerializeField] private Transform _bodyTargetTracker;
    [SerializeField] private Vector3 _bodyPositionOffset = new Vector3(0, 0.5f, 0);
    [SerializeField] private float _bodyAdjustSpeed = 0.1f; // Speed of body adjustment
    [SerializeField] private float _maxYawProportionality = 2f; // Maximum yaw proportionality for body rotation

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
    [SerializeField] private float _bodyRotationSpeed = 0.1f; // Speed of body rotation towards target
    [SerializeField] private float _rotationSpeed = 0.1f; // Speed of rotation towards target

    [SerializeField] private float _attackRange = 20f; // Range of the attack
    [SerializeField] private float _attackDamage = 100f; // Damage of the attack
    [SerializeField] private float _attackCooldown = 3f; // Cooldown time between attacks
    [SerializeField] private float _attackSpeed = 1f; // Speed of the attack animation
    [SerializeField] private float _attackDistanceForward = 1f; // Distance to the target for attack
    [SerializeField] private float _attackDistanceSide = 1f; // Side distance for attack
    [SerializeField] private float _attackHeight = 1f; // Height of the attack
    [SerializeField] private float _attackNoticeDistance = 5f; // Distance to the target for attack notice
    [SerializeField] private AnimationCurve _attackCurve; // Animation curve for the attack

    [SerializeField] private GameObject _attackParticleEffect; // Particle effect for the attack
    
    private Vector3[] _footPositions;
    private bool[] _isStepping;
    private bool[] _hasSteppedThisCycle;
    private bool _allowingZeroIndex = true;
    private float _bodyYaw = 0f; // Yaw rotation of the body
    private float _trackerYaw = 0f; // Yaw rotation of the tracker
    private bool _lastAttackedWithRightLeg = false; // Track if the last attack was with the right leg
    private bool _isAttacking = false; // Track if the spider is currently attacking
    private float _attackCooldownTimer = 0f; // Timer for attack cooldown

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
        _attackCooldownTimer -= Time.deltaTime; // Decrease attack cooldown timer

        // Move body towards target transform
        if (_targetTransform != null && !_isAttacking) {
            Vector3 targetPosition = _targetTransform.position;
            Vector3 direction = (targetPosition - _bodyTargetTracker.position).normalized;
            direction.y = 0; // Only move in the x-z plane because the trackers find height already
            _bodyTargetTracker.position += direction * _moveSpeed * Time.deltaTime;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float currentAngle = _trackerYaw;
            // Figure out the shortest rotation direction
            float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);
            if (angleDifference > 180f) angleDifference -= 360f;
            if (angleDifference < -180f) angleDifference += 360f;
            // Rotate towards the target direction
            float toRotate = currentAngle + angleDifference * _rotationSpeed * Time.deltaTime;
            _trackerYaw = toRotate; // Update tracker yaw
            _bodyTargetTracker.rotation = Quaternion.Euler(0, _trackerYaw, 0); // Rotate tracker based on yaw
        }

        // Update body height and rotation
        Vector3 averagePos = Vector3.zero;
        for (int i = 0; i < _footPositions.Length; i++)
        {
            averagePos += _footPositions[i];
        }
        averagePos /= _footPositions.Length;
        Vector3 bodyDesiredPosition = averagePos + _bodyPositionOffset;
        _body.position = Vector3.Lerp(_body.position, bodyDesiredPosition, Time.deltaTime * _bodyAdjustSpeed);
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

        // Yaw rotates the body based on the difference in left and right leg positions
        float avgProjectionLeft = 0f;
        float avgProjectionRight = 0f;
        // Measuring the average projection of the foot positions on the forward axis of the body
        for (int i = 0; i < _footPositions.Length; i++)
        {
            Vector3 footPosition = _footPositions[i];
            Vector3 bodyForward = _body.forward;
            float projection = Vector3.Dot(footPosition, bodyForward);
            if (i < 4) // Left foot
            {
                avgProjectionLeft += projection;
            }
            else // Right foot
            {
                avgProjectionRight += projection;
            }
        }
        avgProjectionLeft /= 4f;
        avgProjectionRight /= 4f;
        float yaw = Mathf.Clamp(avgProjectionLeft - avgProjectionRight, -_maxYawProportionality, _maxYawProportionality);
        
        _bodyYaw += yaw * _bodyRotationSpeed; // Update body yaw
        
        _body.localRotation = Quaternion.Euler(pitch * _pitchIntensity, _bodyYaw, tilt * _tiltIntensity); // Rotate body based on foot heights

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
                TakeStep(i, footTargetTrackerPosition, footTargetPosition);
            }
        }

        // Check if player is in attack range
        if (_targetTransform != null) {
            float distanceToTarget = Vector3.Distance(_body.position, _targetTransform.position);
            if (distanceToTarget <= _attackNoticeDistance) {
                // Attack the target
                AttackTarget();
            }
        }
    }

    private void AttackTarget() {
        if (_isAttacking) return; // Already attacking
        if (_attackCooldownTimer > 0f) return; // Attack cooldown not finished
        Debug.Log("Attacking target!");
        _attackCooldownTimer += _attackCooldown;
        _isAttacking = true; // Set attacking state
        _lastAttackedWithRightLeg = !_lastAttackedWithRightLeg; // Toggle leg for next attack

        StartCoroutine(AttackTargetCoroutine());
    }

    private IEnumerator AttackTargetCoroutine() {
        float elapsedTime = 0f;
        Vector3 startPosition = _footPositions[_lastAttackedWithRightLeg ? 5 : 0]; // Start position of the attack
        Vector3 endPosition = startPosition + _body.forward * _attackDistanceForward + Vector3.up * _attackHeight + _body.right * _attackDistanceSide * (_lastAttackedWithRightLeg ? -1 : 1); // End position of the attack
        while (elapsedTime < _attackSpeed / 2) {
            elapsedTime += Time.deltaTime;
            float t = _attackCurve.Evaluate(elapsedTime / (_attackSpeed / 2)); // Evaluate the attack curve
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, t); // Move to end position
            _footPositions[_lastAttackedWithRightLeg ? 5 : 0] = newPosition; // Update foot position for attack
            yield return null;
        }

        // Spawn particle effect at foot location
        GameObject particleEffect = Instantiate(_attackParticleEffect, _footPositions[_lastAttackedWithRightLeg ? 5 : 0], Quaternion.identity); // Instantiate particle effect at foot position
        // Assign player collider to particle effect callback list
        ParticleSystem particleEffectScript = particleEffect.GetComponent<ParticleSystem>();
        if (particleEffectScript != null) {
            particleEffectScript.trigger.AddCollider(_targetTransform.GetComponent<Collider>()); // Add player collider to particle effect callback list
            particleEffect.GetComponent<PlayerParticleDamage>().player = _targetTransform.gameObject; // Assign player to particle effect script
        }

        // Damage player if in range
        if (_targetTransform != null) {
            float distanceToTarget = Vector3.Distance(_footPositions[_lastAttackedWithRightLeg ? 5 : 0], _targetTransform.position);
            if (distanceToTarget <= _attackRange) {
                // Apply damage to the target
                if (_targetTransform.GetComponent<HealthManager>() != null) {
                    _targetTransform.GetComponent<HealthManager>().Damage(_attackDamage, _footPositions[_lastAttackedWithRightLeg ? 5 : 0]);
                }
            }
        }   

        elapsedTime = 0f; // Reset elapsed time for attack animation
        while (elapsedTime < _attackSpeed / 2) {
            elapsedTime += Time.deltaTime;
            float t = _attackCurve.Evaluate(1 - (elapsedTime / (_attackSpeed / 2))); // Evaluate the attack curve for return
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, t); // Return to start position
            _footPositions[_lastAttackedWithRightLeg ? 5 : 0] = newPosition; // Update foot position for attack
            yield return null;
        }

        _footPositions[_lastAttackedWithRightLeg ? 5 : 0] = startPosition; // Reset foot position after attack
        _isAttacking = false; // Reset attacking state
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
            if (!(_isAttacking && footIndex != (_lastAttackedWithRightLeg ? 5 : 0))) elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _stepSpeed);
            float height = Mathf.Sin(t * Mathf.PI) * _stepHeight;
            float curveValue = _stepCurve.Evaluate(t);
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, curveValue) + new Vector3(0, height, 0);
            if (_isAttacking && footIndex == (_lastAttackedWithRightLeg ? 5 : 0)) {
                // If attacking, ignore step logic
                newPosition = _footPositions[_lastAttackedWithRightLeg ? 5 : 0];
            }
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
