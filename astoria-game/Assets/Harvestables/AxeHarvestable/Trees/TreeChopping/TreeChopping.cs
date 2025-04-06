using System.Collections.Generic;
using UnityEngine;

public class TreeChopping : MonoBehaviour
{

  private struct TerrainTile
  {

    public Vector3 position;
    public Vector2 size;
    public TerrainData terrainData;
    public TreeInstance[] treeInstances;
    public List<TreeInstance> removedTreeInstances;

  }

  [SerializeField] private GameObject[] _terrainObjects;
  [SerializeField] private Transform _treeParent;
  [SerializeField] private float _treeSearchRadius = 1f;
  [SerializeField] private GameObject[] _treePrefabs;

  private TerrainTile[] _terrainTiles;

  public static TreeChopping Instance { get; private set; }

  public void Start() {
    Instance = this;

    _terrainTiles = new TerrainTile[_terrainObjects.Length];
    for (int i = 0; i < _terrainObjects.Length; i++) {
      Terrain terrain = _terrainObjects[i].GetComponent<Terrain>();
      _terrainTiles[i].terrainData = terrain.terrainData;
      _terrainTiles[i].treeInstances = terrain.terrainData.treeInstances;
      _terrainTiles[i].position = terrain.transform.position;
      _terrainTiles[i].size = new Vector2(terrain.terrainData.size.x, terrain.terrainData.size.z);
      _terrainTiles[i].removedTreeInstances = new List<TreeInstance>();
    }
  }

  /// <summary>
  /// Realizes a terrain tree into a tree prefab.
  /// </summary>
  /// <param name="position">The closest point you can get to the tree. Can be a hit.position on the terrain tree collider.</param>
  /// <returns>The realized tree prefab GameObject.</returns>
  public GameObject RealizeTree(Vector3 position) {
    //Debug.Log("Realizing tree at " + position);

    // Find the tree instance at the given position
    Vector2 position2D = new Vector2(position.x, position.z);
    for (int t = 0; t < _terrainTiles.Length; t++) {
      // Check if the position is within the bounds of the terrain tile
      TerrainTile terrainTile = _terrainTiles[t];
      if (position2D.x < terrainTile.position.x || position2D.x > terrainTile.position.x + terrainTile.size.x ||
          position2D.y < terrainTile.position.z || position2D.y > terrainTile.position.z + terrainTile.size.y) {
        continue;
      }

      //Debug.Log("Found tree's terrain tile");
      for (int i = 0; i < terrainTile.treeInstances.Length; i++) {
        TreeInstance treeInstance = terrainTile.treeInstances[i];
        Vector3 treePosition = Vector3.Scale(treeInstance.position, terrainTile.terrainData.size) + terrainTile.position;

        Vector2 treePosition2D = new Vector2(treePosition.x, treePosition.z);

        if (Vector2.Distance(treePosition2D, position2D) < _treeSearchRadius) {
          // Debug.Log("Found tree at " + treePosition);
          // Remove the tree instance
          TreeInstance _newInstance = new TreeInstance();
          TreeInstance oldInstance = terrainTile.treeInstances[i];
          _newInstance.prototypeIndex = oldInstance.prototypeIndex;
          _newInstance.position = oldInstance.position;
          _newInstance.widthScale = oldInstance.widthScale;
          _newInstance.heightScale = 0;
          _newInstance.color = oldInstance.color;
          _newInstance.lightmapColor = oldInstance.lightmapColor;
          _newInstance.rotation = oldInstance.rotation;
          terrainTile.terrainData.SetTreeInstance(i, _newInstance);

          terrainTile.removedTreeInstances.Add(oldInstance);

          TreeColliderManager.Instance.DisableCollider(treePosition);

          // Instantiate a tree prefab at the tree position
          GameObject treePrefab = Instantiate(_treePrefabs[_newInstance.prototypeIndex], treePosition, Quaternion.Euler(0, treeInstance.rotation * Mathf.Rad2Deg, 0), _treeParent);
          // Set the tree prefab's scale to the tree instance's scale
          treePrefab.transform.localScale = new Vector3(treePrefab.transform.localScale.x * oldInstance.widthScale, treePrefab.transform.localScale.y * oldInstance.heightScale, treePrefab.transform.localScale.z * oldInstance.widthScale);
          return treePrefab;
        }
      }
    }
    return null;
  }

  public void RegrowTree(Vector3 position) {
    //Debug.Log("Regrowing tree at " + position);

    // Find the tree instance at the given position
    for (int t = 0; t < _terrainTiles.Length; t++) {
      TerrainTile terrainTile = _terrainTiles[t];

      // Check if the position is within the bounds of the terrain tile
      Vector2 position2D = new Vector2(position.x, position.z);
      if (position2D.x < terrainTile.position.x || position2D.x > terrainTile.position.x + terrainTile.size.x ||
          position2D.y < terrainTile.position.y || position2D.y > terrainTile.position.y + terrainTile.size.y) {
        continue;
      }

      for (int i = 0; i < terrainTile.removedTreeInstances.Count; i++) {
        TreeInstance treeInstance = terrainTile.removedTreeInstances[i];
        Vector3 treePosition = Vector3.Scale(treeInstance.position, terrainTile.terrainData.size) + transform.position;

        if (Vector3.Distance(treePosition, position) < _treeSearchRadius) {
          //Debug.Log("Found tree at " + treePosition);
          // Regrow the tree instance
          TreeInstance _newInstance = terrainTile.removedTreeInstances[i];
          _newInstance.prototypeIndex = terrainTile.removedTreeInstances[i].prototypeIndex;
          _newInstance.position = terrainTile.removedTreeInstances[i].position;
          _newInstance.widthScale = terrainTile.removedTreeInstances[i].widthScale;
          _newInstance.heightScale = terrainTile.removedTreeInstances[i].heightScale;
          _newInstance.color = terrainTile.removedTreeInstances[i].color;
          _newInstance.lightmapColor = terrainTile.removedTreeInstances[i].lightmapColor;
          _newInstance.rotation = terrainTile.removedTreeInstances[i].rotation;
          terrainTile.terrainData.SetTreeInstance(i, _newInstance);

          // Update tree collider
          TreeColliderManager.Instance.EnableCollider(treePosition);

          return;
        }
      }
    }
  }

}
