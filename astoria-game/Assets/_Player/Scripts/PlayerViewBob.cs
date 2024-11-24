using KinematicCharacterController;
using Player;
using UnityEngine;

public class PlayerViewBob : MonoBehaviour
{

  [SerializeField] private PlayerController _playerController;
  [SerializeField] private Transform _cameraTransform;
  [SerializeField] private AnimationCurve _viewBobVerticalCurve;
  [SerializeField] private float _verticalMultiplier;
  [SerializeField] private AnimationCurve _viewBobHorizontalCurve;
  [SerializeField] private float _horizontalMultiplier;
  [SerializeField] private AnimationCurve _viewBobVelocityCurve;
  [SerializeField] private AnimationCurve _viewBobVelocityAmplitudeCurve;
  [Range(0.1f, 10f)] [SerializeField] private float _velocityRange = 1f;
  [SerializeField] private float _viewBobFrequency = 1f;

  [Header("Noise")]
  [SerializeField] private float _noiseFrequency = 1f;
  [SerializeField] private float _noiseAmplitude = 1f;

  private KinematicCharacterMotor _motor;
  private Vector3 _baseVelocity;
  private float _timeSinceLastStep;
  private bool _stepRight;
  private float _currentCurvePosition = 1;

  private void Start() {
    _motor = _playerController.Motor;
  }

  private void Update() {
    _baseVelocity = _motor.BaseVelocity;
    Vector2 velocity = new Vector2(_baseVelocity.x, _baseVelocity.z);
    float frequency = _viewBobFrequency * _viewBobVelocityCurve.Evaluate(velocity.magnitude / _velocityRange);

    _currentCurvePosition += Time.deltaTime * frequency;
    _timeSinceLastStep += Time.deltaTime;

    ProcessStep();

    if (_motor.GroundingStatus.IsStableOnGround) {
      if (_baseVelocity.magnitude > 0.1f) {
        if (_timeSinceLastStep > 1f / frequency && _currentCurvePosition >= 1) {
          _timeSinceLastStep = 0;
          _currentCurvePosition = 0;
          _stepRight = !_stepRight;
        }
      }
    }

  }

  private void ProcessStep() {
    Vector2 velocity = new Vector2(_baseVelocity.x, _baseVelocity.z);
    float multiplier = _viewBobVelocityAmplitudeCurve.Evaluate(velocity.magnitude / _velocityRange);
    float verticalOffset = _viewBobVerticalCurve.Evaluate(_currentCurvePosition) * _verticalMultiplier * multiplier;
    float horizontalOffset = _viewBobHorizontalCurve.Evaluate(_currentCurvePosition) * _horizontalMultiplier * multiplier;

    if (!_stepRight) {
      horizontalOffset = -horizontalOffset;
    }

    float magnitude = _viewBobVelocityCurve.Evaluate(velocity.magnitude / _velocityRange);

    float noise = Mathf.PerlinNoise(Time.time * _noiseFrequency, 0) * _noiseAmplitude * magnitude;
    horizontalOffset += noise;
    noise = Mathf.PerlinNoise(0, Time.time * _noiseFrequency) * _noiseAmplitude * magnitude;
    verticalOffset += noise;

    _cameraTransform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0);
  }

}
