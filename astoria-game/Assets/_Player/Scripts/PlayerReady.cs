using Mirror;
using UnityEngine;

public class PlayerReady : NetworkBehaviour
{
  [SerializeField] private GameObject _playerWorldmodel;
  [SerializeField] private GameObject _playerViewmodel;
  public void InitializeLocalPlayer() {
    SetLayerAllChildren(_playerViewmodel.transform, LayerMask.NameToLayer("LocalPlayerWorldmodel"));
    SetLayerAllChildren(_playerWorldmodel.transform, LayerMask.NameToLayer("LocalPlayerViewmodel"));
  }
  
  void SetLayerAllChildren(Transform root, int layer)
  {
    var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
    foreach (var child in children)
    {
      child.gameObject.layer = layer;
    }
  }
}
