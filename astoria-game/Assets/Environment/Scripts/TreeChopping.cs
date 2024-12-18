using UnityEngine;

public class TreeChopping : MonoBehaviour
{

  [SerializeField] private TerrainData _terrainData;
  [SerializeField] private float _treeSearchRadius = 1f;
  [SerializeField] private GameObject _treePrefab;

  private TreeInstance[] _treeInstances;

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

        // Instantiate a tree prefab at the tree position
        Instantiate(_treePrefab, treePosition, Quaternion.identity);
        return;
      }
    }
  }

  public void RegrowTree(Vector3 position) {
    Debug.Log("Regrowing tree at " + position);

    // Find the tree instance at the given position
    for (int i = 0; i < _treeInstances.Length; i++) {
      TreeInstance treeInstance = _treeInstances[i];
      Vector3 treePosition = Vector3.Scale(treeInstance.position, _terrainData.size) + transform.position;

      if (Vector3.Distance(treePosition, position) < _treeSearchRadius) {
        Debug.Log("Found tree at " + treePosition);
        // Regrow the tree instance
        TreeInstance _newInstance = new TreeInstance();
        _newInstance.prototypeIndex = _treeInstances[i].prototypeIndex;
        _newInstance.position = _treeInstances[i].position;
        _newInstance.widthScale = _treeInstances[i].widthScale;
        _newInstance.heightScale = _treeInstances[i].heightScale;
        _newInstance.color = _treeInstances[i].color;
        _newInstance.lightmapColor = _treeInstances[i].lightmapColor;
        _newInstance.rotation = _treeInstances[i].rotation;
        _terrainData.SetTreeInstance(i, _newInstance);
        return;
      }
    }
  }

}
