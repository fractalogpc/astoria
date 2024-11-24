using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
  [SerializeField] private Transform target;
  [SerializeField] private bool IgnoreXAxis;
  
  private void Update()
  {
    if (target)
    {
      if (IgnoreXAxis)
      {
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
      } else {
        transform.LookAt(target);
      }
    }
  }
}
