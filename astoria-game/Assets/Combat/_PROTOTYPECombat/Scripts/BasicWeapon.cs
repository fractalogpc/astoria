using UnityEngine;

public class BasicWeapon : MonoBehaviour
{

  [SerializeField] private float _damage = 10f;
  [SerializeField] private float _range = 100f;
  [SerializeField] private float _fireRate = 15f;

  private float _nextTimeToFire = 0f;

  private void Update() {
    if (Input.GetMouseButtonDown(0) && Time.time >= _nextTimeToFire) {
      _nextTimeToFire = Time.time + 1f / _fireRate;
      Shoot();
    }
  }

  private void Shoot() {
    RaycastHit hit;
    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, _range)) {
      Health target = hit.transform.GetComponent<Health>();
      if (target != null) {
        target.TakeDamage((int)_damage);
      }
    }
  }
}
