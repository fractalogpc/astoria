using UnityEngine;

public class UnpackChildren : MonoBehaviour, IStartExecution
{
  public void Unpack()
  {
    Debug.Log(transform.childCount);
    int children = transform.childCount;
    for (int i = 0; i < children; i++)
    {
      transform.GetChild(0).parent = null;
    }
  }

  public void InitializeStart()
  {
    Unpack();
  }
}
