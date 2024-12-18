using UnityEngine;
using System.Collections.Generic;

public class TreeColliderManager : MonoBehaviour
{

  private struct ColliderGrid
  {

    public Vector2 centerPosition;
    public Vector3[] colliders;
    public bool active;
    public bool[] disabledColliders;
    
  }

  // Must contain reference to all tree instances on its terrain
  [SerializeField] private TerrainData _terrainData;
  [SerializeField] private GameObject _colliderPrefab;

  [SerializeField] private float _activationRadius = 100f;
  [SerializeField] private int _gridDivisions = 10;

  private TreeInstance[] _treeInstances;

  private ColliderGrid[] _grid;

  private Dictionary<Vector3, GameObject> _activeColliders = new Dictionary<Vector3, GameObject>();
  private List<GameObject> _unusedColliders = new List<GameObject>();

  public static TreeColliderManager Instance { get; private set; }

  private void Start() {
    Instance = this;
    _treeInstances = _terrainData.treeInstances;
    CreateGrid();
  }

  public void DisableCollider(Vector3 position) {
    for (int i = 0; i < _grid.Length; i++) {
      ColliderGrid gridCell = _grid[i];
      
      // Check if the position is in the grid cell
      Vector2 diff = new Vector2(position.x, position.z) - gridCell.centerPosition;

      float gridCellSize = _terrainData.size.x / _gridDivisions;

      if (Mathf.Max(Mathf.Abs(diff.x), Mathf.Abs(diff.y)) >= gridCellSize / 2f) continue;

      for (int j = 0; j < gridCell.colliders.Length; j++) {
        if (gridCell.disabledColliders[j]) continue;
        if (gridCell.colliders[j] == position) {
          gridCell.disabledColliders[j] = true;
          _activeColliders[position].SetActive(false);
          _unusedColliders.Add(_activeColliders[position]);
          _activeColliders.Remove(position);
          break;
        }
      }
    }
  }

  public void EnableCollider(Vector3 position) {
    for (int i = 0; i < _grid.Length; i++) {
      ColliderGrid gridCell = _grid[i];
      
      // Check if the position is in the grid cell
      Vector2 diff = new Vector2(position.x, position.z) - gridCell.centerPosition;

      float gridCellSize = _terrainData.size.x / _gridDivisions;

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
            GameObject collider = Instantiate(_colliderPrefab, gridCell.colliders[j], Quaternion.identity);
            _activeColliders.Add(gridCell.colliders[j], collider);
          }

          break;
        }
      }
    }
  }

  private void Update() {
    // Attach this to local player reference
    Vector3 playerPosition = transform.position;

    for (int i = 0; i < _grid.Length; i++) {
      ColliderGrid gridCell = _grid[i];
      Vector2 gridCellPosition = gridCell.centerPosition;

      if (Vector2.Distance(new Vector2(playerPosition.x, playerPosition.z), gridCellPosition) < _activationRadius) {
        if (!gridCell.active) {
          gridCell.active = true;
          for (int j = 0; j < gridCell.colliders.Length; j++) {
            if (gridCell.disabledColliders[j]) continue;
            
            if (_unusedColliders.Count > 0) {
              GameObject collider = _unusedColliders[0];
              _unusedColliders.RemoveAt(0);
              collider.SetActive(true);
              collider.transform.position = gridCell.colliders[j];
              _activeColliders.Add(gridCell.colliders[j], collider);
            } else {
              GameObject collider = Instantiate(_colliderPrefab, gridCell.colliders[j], Quaternion.identity);
              _activeColliders.Add(gridCell.colliders[j], collider);
            }
          }
        }
      } else {
        if (gridCell.active) {
          gridCell.active = false;

          for (int j = 0; j < gridCell.colliders.Length; j++) {
            if (gridCell.disabledColliders[j]) continue;

            _unusedColliders.Add(_activeColliders[gridCell.colliders[j]]);
            _activeColliders.Remove(gridCell.colliders[j]);
          }
        }
      }
    }
  }

  private void CreateGrid() {
    _grid = new ColliderGrid[_gridDivisions * _gridDivisions];
    Vector3 terrainSize = _terrainData.size;
    Vector3 terrainPosition = transform.position;

    for (int i = 0; i < _gridDivisions; i++) {
      for (int j = 0; j < _gridDivisions; j++) {
        ColliderGrid gridCell = new ColliderGrid();
        gridCell.centerPosition = new Vector2(i * terrainSize.x / _gridDivisions, j * terrainSize.z / _gridDivisions) + new Vector2(terrainPosition.x, terrainPosition.z);
        gridCell.active = false;

        // Get subset of tree instances within the grid cell
        List<TreeInstance> treeInstancesInCell = new List<TreeInstance>();
        for (int k = 0; k < _treeInstances.Length; k++) {
          Vector3 treePosition = Vector3.Scale(_treeInstances[k].position, terrainSize) + terrainPosition;
          Vector2 treePosition2D = new Vector2(treePosition.x, treePosition.z);

          Vector2 diff = treePosition2D - gridCell.centerPosition;
          float gridCellSize = terrainSize.x / _gridDivisions;

          if (Mathf.Max(Mathf.Abs(diff.x), Mathf.Abs(diff.y)) < gridCellSize / 2f) {
            treeInstancesInCell.Add(_treeInstances[k]);
          }
        }

        gridCell.colliders = new Vector3[treeInstancesInCell.Count];
        gridCell.disabledColliders = new bool[treeInstancesInCell.Count];

        for (int k = 0; k < gridCell.colliders.Length; k++) {
          gridCell.colliders[k] = Vector3.Scale(treeInstancesInCell[k].position, terrainSize) + terrainPosition;
        }

        _grid[i * _gridDivisions + j] = gridCell;
      }
    }
  }

}