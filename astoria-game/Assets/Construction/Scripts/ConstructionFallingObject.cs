using UnityEngine;

public class ConstructionFallingObject : MonoBehaviour
{
  public ConstructableObjectData Data;
  private Rigidbody _rb;
  private float _timer = 0;
  private const float MaxTime = 5;
  private void OnEnable() {
    _rb = GetComponent<Rigidbody>();
  }

  private void Update()
  {
    _timer += Time.deltaTime;
    if (_timer >= MaxTime || (_timer >= 0.5f && _rb.linearVelocity.magnitude < 0.1f && _rb.angularVelocity.magnitude < 0.1f))
    {
      SwitchObjectToPermanent();
    }
  }

  private void SwitchObjectToPermanent() {
    GameObject permanent = Instantiate(Data.FinalPrefab, transform.position, transform.rotation);
    Destroy(gameObject);
  }
}
