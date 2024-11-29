using Mirror.BouncyCastle.Asn1.X509;
using UnityEngine;

public class LookAtTarget : MonoBehaviour, IStartExecution
{
  [SerializeField] private bool IgnoreXAxis;

  private Transform target;

  private bool _tryInitialize;
  private bool _initialized;

  public void InitializeStart() {
    ResourceHolder.Instance.GameStateHandler.AddOnStateEnter(GameStateHandler.GameState.Playing, () => {
      Initialize();
    });
  }
  
  private void Initialize() {
    _tryInitialize = true;
    GameObject targetObject = LocalPlayerReference.Instance.LocalPlayer;

    if (targetObject != null) {
      target = targetObject.transform;
      _initialized = true;
    }
  }

  private void Update()
  {
    if (_initialized)
    {
      if (IgnoreXAxis)
      {
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
      } else {
        transform.LookAt(target);
      }
    } else {
      if (_tryInitialize) {
        Initialize();
      }
    }
  }
}
