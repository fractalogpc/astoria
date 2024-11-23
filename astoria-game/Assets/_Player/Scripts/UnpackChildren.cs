using UnityEngine;

public class UnpackChildren : MonoBehaviour, IStartExecution
{

  [SerializeField] private bool _destroy = true;

  public void Unpack()
  {
    int children = transform.childCount;
    for (int i = 0; i < children; i++)
    {
      transform.GetChild(0).SetParent(null);
    }
  }

  public void InitializeStart()
  {
    Unpack();

    if (_destroy)
    {
      Destroy(gameObject);
    }
  }
}
