using System.Collections.Generic;
using UnityEngine;

public class TreeChopping : MonoBehaviour
{

  [SerializeField] private TerrainData _terrainData;
  [SerializeField] private TerrainCollider _terrainCollider;
  [SerializeField] private float _treeSearchRadius = 1f;
  [SerializeField] private GameObject _treePrefab;

  private TreeInstance[] _treeInstances;

  private List<TreeInstance> _removedTreeInstances = new List<TreeInstance>();

  public static TreeChopping Instance { get; private set; }

  public void Start() {
    Instance = this;
    _treeInstances = _terrainData.treeInstances;
  }

  public void InteractTree(Vector3 position) {
    Debug.Log("Chopping tree at " + position);

    // Find the tree instance at the given position
    Vector2 position2D = new Vector2(position.x, position.z);
    for (int i = 0; i < _treeInstances.Length; i++) {
      TreeInstance treeInstance = _treeInstances[i];
      Vector3 treePosition = Vector3.Scale(treeInstance.position, _terrainData.size) + transform.position;

      Vector2 treePosition2D = new Vector2(treePosition.x, treePosition.z);

      if (Vector2.Distance(treePosition2D, position2D) < _treeSearchRadius) {
        Debug.Log("Found tree at " + treePosition);
        // Remove the tree instance
        TreeInstance _newInstance = new TreeInstance();
        _newInstance.prototypeIndex = _treeInstances[i].prototypeIndex;
        _newInstance.position = _treeInstances[i].position;
        _newInstance.widthScale = _treeInstances[i].widthScale;
        _newInstance.heightScale = 0;
        _newInstance.color = _treeInstances[i].color;
        _newInstance.lightmapColor = _treeInstances[i].lightmapColor;
        _newInstance.rotation = _treeInstances[i].rotation;
        _terrainData.SetTreeInstance(i, _newInstance);

        // Below is alternative way to do it, more expensive theoretically because it sets whole array.
        // Above is rendering change only and better if not using terrain colliders.
        // Terrain tree colliders are expensive to update and should not be used.
        // See TreeColliderManager.

        // // Create new list of tree instances without the chopped tree
        // TreeInstance[] newTreeInstances = new TreeInstance[_treeInstances.Length - 1];
        // int j = 0;
        // for (int k = 0; k < _treeInstances.Length; k++) {
        //   if (k != i) {
        //     newTreeInstances[j] = _treeInstances[k];
        //     j++;
        //   }
        // }

        // _removedTreeInstances.Add(_treeInstances[i]);

        // // Set the new tree instances
        // _treeInstances = newTreeInstances;
        // _terrainData.SetTreeInstances(newTreeInstances, false);

        // _terrainCollider.enabled = false;
        // _terrainCollider.enabled = true;

        // Update tree collider
        TreeColliderManager.Instance.DisableCollider(treePosition);

        // Instantiate a tree prefab at the tree position
        Instantiate(_treePrefab, treePosition, Quaternion.identity);
        return;
      }
    }
  }

  public void RegrowTree(Vector3 position) {
    Debug.Log("Regrowing tree at " + position);

    // Find the tree instance at the given position
    for (int i = 0; i < _removedTreeInstances.Count; i++) {
      TreeInstance treeInstance = _removedTreeInstances[i];
      Vector3 treePosition = Vector3.Scale(treeInstance.position, _terrainData.size) + transform.position;

      if (Vector3.Distance(treePosition, position) < _treeSearchRadius) {
        Debug.Log("Found tree at " + treePosition);
        // Regrow the tree instance
        TreeInstance _newInstance = _removedTreeInstances[i];
        _newInstance.prototypeIndex = _removedTreeInstances[i].prototypeIndex;
        _newInstance.position = _removedTreeInstances[i].position;
        _newInstance.widthScale = _removedTreeInstances[i].widthScale;
        _newInstance.heightScale = _removedTreeInstances[i].heightScale;
        _newInstance.color = _removedTreeInstances[i].color;
        _newInstance.lightmapColor = _removedTreeInstances[i].lightmapColor;
        _newInstance.rotation = _removedTreeInstances[i].rotation;
        _terrainData.SetTreeInstance(i, _newInstance);

        // // Create new list of tree instances with the regrown tree
        // TreeInstance[] newTreeInstances = new TreeInstance[_treeInstances.Length + 1];
        // for (int j = 0; j < _treeInstances.Length; j++) {
        //   newTreeInstances[j] = _treeInstances[j];
        // }
        // newTreeInstances[_treeInstances.Length] = _newInstance;

        // _removedTreeInstances.RemoveAt(i);

        // // Set the new tree instances
        // _treeInstances = newTreeInstances;
        // _terrainData.SetTreeInstances(newTreeInstances, false);

        // _terrainCollider.enabled = false;
        // _terrainCollider.enabled = true;

        // Update tree collider
        TreeColliderManager.Instance.EnableCollider(treePosition);

        return;
      }
    }
  }

}
