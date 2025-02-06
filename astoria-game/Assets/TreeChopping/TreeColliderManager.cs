using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;

public class TreeColliderManager : MonoBehaviour
{

  private struct ColliderGrid
  {

    public Vector2 centerPosition;
    public Vector3[] colliders;
    public bool active;
    public bool[] disabledColliders;
    
  }

  private struct TerrainTile
  {

    public Vector3 position;
    public Vector2 centerPosition;
    public TerrainData terrainData;
    public Vector3[] treeInstances;
    public ColliderGrid[] grid;
  }

  // Must contain reference to all tree instances on its terrain
  [SerializeField] private GameObject[] _terrainObjects;
  [SerializeField] private GameObject _colliderPrefab;
  [SerializeField] private Transform _colliderParent;

  [SerializeField] private float _activationRadius = 100f;
  [SerializeField] private float _tileActivationRadius = 1000f;
  [SerializeField] private int _gridDivisions = 10;

  private TerrainTile[] _terrainTiles;

  private Dictionary<Vector3, GameObject> _activeColliders = new Dictionary<Vector3, GameObject>();
  private List<GameObject> _unusedColliders = new List<GameObject>();

  private Transform _localPlayer;

  public static TreeColliderManager Instance { get; private set; }

  private IEnumerator Start() {
    Instance = this;

    _terrainTiles = new TerrainTile[_terrainObjects.Length];
    for (int i = 0; i < _terrainObjects.Length; i++) {
      Terrain terrain = _terrainObjects[i].GetComponent<Terrain>();
      _terrainTiles[i].terrainData = terrain.terrainData;
      TreeInstance[] instances = _terrainTiles[i].terrainData.treeInstances;
      _terrainTiles[i].treeInstances = new Vector3[instances.Length];
      for (int j = 0; j < instances.Length; j++) {
        _terrainTiles[i].treeInstances[j] = instances[j].position;
      }
      _terrainTiles[i].position = terrain.transform.position;
      _terrainTiles[i].centerPosition = new Vector2(terrain.transform.position.x + terrain.terrainData.size.x / 2, terrain.transform.position.z + terrain.terrainData.size.z / 2);
      CreateGrid(i);
    }

    while (_localPlayer == null) {
      yield return null;
      _localPlayer = PlayerInstance.Instance?.transform;
    }
  }

  public void DisableCollider(Vector3 position) {
    for (int t = 0; t < _terrainTiles.Length; t++) {
      TerrainTile terrainTile = _terrainTiles[t];
      TerrainData terrainData = terrainTile.terrainData;

      // Check if the position is in the terrain tile
      Vector2 diff = new Vector2(position.x, position.z) - terrainTile.centerPosition;

      float terrainSize = terrainData.size.x;
      if (Mathf.Max(Mathf.Abs(diff.x), Mathf.Abs(diff.y)) >= terrainSize / 2f) continue;

      for (int i = 0; i < terrainTile.grid.Length; i++) {
        ColliderGrid gridCell = terrainTile.grid[i];
        
        // Check if the position is in the grid cell
        diff = new Vector2(position.x, position.z) - gridCell.centerPosition;

        float gridCellSize = terrainData.size.x / _gridDivisions;

        if (Mathf.Max(Mathf.Abs(diff.x), Mathf.Abs(diff.y)) >= gridCellSize / 2f) continue;

        for (int j = 0; j < gridCell.colliders.Length; j++) {
          if (gridCell.disabledColliders[j]) continue;
          if (gridCell.colliders[j] == position) {
            gridCell.disabledColliders[j] = true;
            if (!gridCell.active) break;
            _activeColliders[position].SetActive(false);
            _unusedColliders.Add(_activeColliders[position]);
            _activeColliders.Remove(position);
            break;
          }
        }
      }

      break;
    }
  }

  public void EnableCollider(Vector3 position) {
    for (int t = 0; t < _terrainTiles.Length; t++) {
      TerrainTile terrainTile = _terrainTiles[t];
      TerrainData terrainData = terrainTile.terrainData;

      // Check if the position is in the terrain tile
      Vector2 diff = new Vector2(position.x, position.z) - terrainTile.centerPosition;

      float terrainSize = terrainData.size.x;
      if (Mathf.Max(Mathf.Abs(diff.x), Mathf.Abs(diff.y)) >= terrainSize / 2f) continue;

      for (int i = 0; i < terrainTile.grid.Length; i++) {
        ColliderGrid gridCell = terrainTile.grid[i];
        
        // Check if the position is in the grid cell
        diff = new Vector2(position.x, position.z) - gridCell.centerPosition;

        float gridCellSize = terrainData.size.x / _gridDivisions;

        if (Mathf.Max(Mathf.Abs(diff.x), Mathf.Abs(diff.y)) >= gridCellSize / 2f) continue;

        for (int j = 0; j < gridCell.colliders.Length; j++) {
          if (!gridCell.disabledColliders[j]) continue;
          if (gridCell.colliders[j] == position) {
            gridCell.disabledColliders[j] = false;
            
            if (_unusedColliders.Count > 0) {
              GameObject collider = _unusedColliders[0];
              _unusedColliders.RemoveAt(0);
              collider.SetActive(true);
              collider.transform.position = gridCell.colliders[j];
              _activeColliders.Add(gridCell.colliders[j], collider);
            } else {
              GameObject collider = Instantiate(_colliderPrefab, gridCell.colliders[j], Quaternion.identity, _colliderParent);
              _activeColliders.Add(gridCell.colliders[j], collider);
            }

            break;
          }
        }
      }

      break;
    }
  }

  private void Update() {
    if (_localPlayer == null) return;

    // Attach this to local player reference
    Vector3 playerPosition = _localPlayer.position;

    for (int t = 0; t < _terrainTiles.Length; t++) {
      TerrainTile terrainTile = _terrainTiles[t];

      // Check if the player is within the activation radius of the terrain tile
      if (Vector2.Distance(new Vector2(playerPosition.x, playerPosition.z), terrainTile.centerPosition) > _tileActivationRadius) {
        int activeGrids = 0;
        for (int i = 0; i < terrainTile.grid.Length; i++) {
          if (terrainTile.grid[i].active) activeGrids++;
        }

        if (activeGrids == 0) continue;
      }
      Debug.Log("Terrain tile " + terrainTile.centerPosition + " is active");
      for (int i = 0; i < terrainTile.grid.Length; i++) {
        ColliderGrid gridCell = terrainTile.grid[i];
        Vector2 gridCellPosition = gridCell.centerPosition;

        if (Vector2.Distance(new Vector2(playerPosition.x, playerPosition.z), gridCellPosition) < _activationRadius) {
          if (!gridCell.active) { 
            terrainTile.grid[i].active = true;
            for (int j = 0; j < gridCell.colliders.Length; j++) {
              if (gridCell.disabledColliders[j]) continue;
              
              if (_unusedColliders.Count > 0) {
                GameObject collider = _unusedColliders[0];
                _unusedColliders.RemoveAt(0);
                collider.SetActive(true);
                collider.transform.position = gridCell.colliders[j];
                _activeColliders.Add(gridCell.colliders[j], collider);
              } else {
                GameObject collider = Instantiate(_colliderPrefab, gridCell.colliders[j], Quaternion.identity, _colliderParent);
                _activeColliders.Add(gridCell.colliders[j], collider);
              }
            }
          }
        } else {
          if (gridCell.active) {
            terrainTile.grid[i].active = false;

            for (int j = 0; j < gridCell.colliders.Length; j++) {
              if (gridCell.disabledColliders[j]) continue;

              _unusedColliders.Add(_activeColliders[gridCell.colliders[j]]);
              _activeColliders.Remove(gridCell.colliders[j]);
            }
          }
        }
      }
    }
  }

  private void CreateGrid(int index) {
    ColliderGrid[] grid = new ColliderGrid[_gridDivisions * _gridDivisions];
    TerrainTile terrainTile = _terrainTiles[index];
    Vector3 terrainSize = terrainTile.terrainData.size;
    Vector3 terrainPosition = terrainTile.position;
    Vector3[] treeInstances = terrainTile.treeInstances;

    for (int i = 0; i < _gridDivisions; i++) {
      for (int j = 0; j < _gridDivisions; j++) {
        ColliderGrid gridCell = new ColliderGrid();
        gridCell.centerPosition = new Vector2(i * terrainSize.x / _gridDivisions, j * terrainSize.z / _gridDivisions) + new Vector2(terrainPosition.x, terrainPosition.z);
        gridCell.active = false;

        // Get subset of tree instances within the grid cell
        List<Vector3> treeInstancesInCell = new List<Vector3>();
        for (int k = 0; k < treeInstances.Length; k++) {
          Vector3 treePosition = Vector3.Scale(treeInstances[k], terrainSize) + terrainPosition;
          Vector2 treePosition2D = new Vector2(treePosition.x, treePosition.z);

          Vector2 diff = treePosition2D - gridCell.centerPosition;
          float gridCellSize = terrainSize.x / _gridDivisions;

          if (Mathf.Max(Mathf.Abs(diff.x), Mathf.Abs(diff.y)) < gridCellSize / 2f) {
            treeInstancesInCell.Add(treeInstances[k]);
          }
        }

        gridCell.colliders = new Vector3[treeInstancesInCell.Count];
        gridCell.disabledColliders = new bool[treeInstancesInCell.Count];

        for (int k = 0; k < gridCell.colliders.Length; k++) {
          gridCell.colliders[k] = Vector3.Scale(treeInstancesInCell[k], terrainSize) + terrainPosition;
        }

        grid[i * _gridDivisions + j] = gridCell;
      }
    }

    _terrainTiles[index].grid = grid;
  }

}