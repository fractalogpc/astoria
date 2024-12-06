using Mirror;
using UnityEngine;

public class InitalizePlayer : NetworkBehaviour
{
  [SerializeField] private GameObject _playerWorldmodel;
  [SerializeField] private GameObject _playerViewmodel;
  public void InitializeLocalPlayer() {
    SetLayerAllChildren(_playerWorldmodel.transform, LayerMask.NameToLayer("LocalPlayerWorldmodel"));
    SetLayerAllChildren(_playerViewmodel.transform, LayerMask.NameToLayer("LocalPlayerViewmodel"));
  }
  public void InitalizeNetworkedPlayer() {
    Debug.Log($"{gameObject.name} is a networked player. Use this method to disable objects that are not needed for networked players.");
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
