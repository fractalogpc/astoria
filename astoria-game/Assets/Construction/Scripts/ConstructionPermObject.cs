using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConstructionPermObject : MonoBehaviour
{
  public ConstructableObjectData Data;

  public List<GameObject> _objects = new List<GameObject>();
  private GameObject _parent;

  public void AddObjectToList(GameObject obj) {
    _objects.Add(obj);
  }

  public void RemoveObjectFromList(GameObject obj) {
    _objects.Remove(obj);
  }

  public List<GameObject> GetObjects(int depth) {
    if (depth > 99) return new List<GameObject>();

    foreach (var obj in _objects.ToList()) { // Prevents an error
      if (obj.GetComponent<ConstructionPermObject>()) _objects.AddRange(obj.GetComponent<ConstructionPermObject>().GetObjects(depth++));
    }
    return _objects;
  }

  public void OnObjectPlaced() {
    Ray ray = new Ray(transform.position, Vector3.down);
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit, 0.5f, LayerMask.GetMask("Construction"))) {
      if (hit.collider.transform.root.GetComponent<ConstructionPermObject>()) {
        hit.collider.transform.root.GetComponent<ConstructionPermObject>().AddObjectToList(gameObject);

        _parent = hit.collider.transform.root.gameObject;
      }
    }
  }

  public void OnObjectRemoved() {
    if (_parent) {
      _parent.GetComponent<ConstructionPermObject>().RemoveObjectFromList(gameObject);
    }
  }
}
