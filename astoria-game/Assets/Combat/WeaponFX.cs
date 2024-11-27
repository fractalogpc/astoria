using UnityEngine;
using System.Collections;

public class WeaponFX : MonoBehaviour
{

  [SerializeField] private Transform _slide;
  [SerializeField] private Transform _muzzle;

  [Header("Slide Settings")]
  [SerializeField] private float _slideForce = 0.1f;
  [SerializeField] private float _slideDuration = 0.1f;
  [SerializeField] private AnimationCurve _slideCurve;

  [Header("Sound Settings")]
  [SerializeField] private AudioClip _fireSound;

  [Header("Shell Settings")]
  [SerializeField] private GameObject _shellPrefab;
  [SerializeField] private float _shellLifetime = 1.0f;
  [SerializeField] private Transform _shellEjectPoint;
  [SerializeField] private Transform _shellEjectDirection;
  [SerializeField] private float _shellForceMultMin = 1f;
  [SerializeField] private float _shellForceMultMax = 2f;
  [SerializeField] private float _shellForceDirectionRandomness = 0.1f;
  [SerializeField] private float _shellTorqueMultMin = 1f;
  [SerializeField] private float _shellTorqueMultMax = 2f;
  [SerializeField] private Vector3 _shellTorqueVector;
  [SerializeField] private float _shellTorqueDirectionRandomness = 0.1f;

  [Header("Muzzle Flash Settings")]
  [SerializeField] private ParticleSystem _muzzleFlash;
  [SerializeField] private ParticleSystem _muzzleSmoke;

  public void PlayFireFX() {
    if (_slide != null && _muzzle != null && _shellPrefab != null && _muzzleFlash != null && _muzzleSmoke != null) {
      // Play slide animation
      PlaySlideAnim();
      
      // Play shell ejection
      PlayShellEjection();
  
      // Play muzzle flash
      PlayMuzzleFlash();
    }

    if (_fireSound != null) {
      // Play fire sound
      PlayFireSound();
    }
  }

  private void PlaySlideAnim() {
    StopCoroutine(SlideAnim());
    StartCoroutine(SlideAnim());
  }

  private void PlayShellEjection() {
    GameObject shell = Instantiate(_shellPrefab, _shellEjectPoint.position, Quaternion.identity);
    Destroy(shell, _shellLifetime);
    Rigidbody shellRb = shell.GetComponent<Rigidbody>();
    Vector3 shellForce = _shellEjectDirection.position - _shellEjectPoint.position;
    shellForce += Random.insideUnitSphere * _shellForceDirectionRandomness;
    shellRb.AddForce(shellForce.normalized * ((_shellForceMultMax - _shellForceMultMin) * Random.value + _shellForceMultMin), ForceMode.Impulse);
    Vector3 shellTorque = _shellTorqueVector + Random.insideUnitSphere * _shellTorqueDirectionRandomness;
    shellRb.AddTorque(shellTorque * ((_shellTorqueMultMax - _shellTorqueMultMin) * Random.value + _shellTorqueMultMin), ForceMode.Impulse);
  }

  private void PlayFireSound() {
    AudioSource.PlayClipAtPoint(_fireSound, transform.position);
  }

  private void PlayMuzzleFlash() {
    _muzzleFlash.Play();
    _muzzleSmoke.Play();
  }

  private IEnumerator SlideAnim() {
    float t = 0;
    _slide.localPosition = Vector3.zero;
    Vector3 startPos = _slide.localPosition;
    Vector3 endPos = startPos + Vector3.back * _slideForce;

    while (t < _slideDuration) {
      t += Time.deltaTime;
      float p = _slideCurve.Evaluate(t / _slideDuration);
      _slide.localPosition = Vector3.Lerp(startPos, endPos, p);
      yield return null;
    }

    _slide.localPosition = startPos;
  }
}
