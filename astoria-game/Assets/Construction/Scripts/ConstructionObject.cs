using System.Collections.Generic;
using UnityEngine;
using Construction;

/// <summary>
/// This class goes on the permenant construction objects including both components and props.
/// Its main use is when deleting an object and for storing props that are on top of objects.
/// </summary>
public class ConstructionObject : MonoBehaviour {
  
  public List<ConstructionObject> parents;
  public List<ConstructionObject> children;

  public void OnPlace() {

  }

  public void Delete() {
    Debug.Log("Try deleting");
    if (GetComponent<ConstructionComponent>()) {
      GetComponent<ConstructionComponent>().DestroyComponent();
    }

    Destroy(this.gameObject);
  }
};
