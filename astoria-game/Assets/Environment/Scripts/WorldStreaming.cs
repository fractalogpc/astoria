using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[InitializeOnLoad]
public class EditorUpdate
{

  static EditorUpdate()
  {
    EditorApplication.update += Update;
  }

  static void Update()
  {
    if (!Application.isPlaying)
    {
      foreach (var worldStreaming in GameObject.FindObjectsByType<WorldStreaming>(FindObjectsSortMode.None))
      {
        worldStreaming.EditorUpdate();
      }
    }
  }

}

[CustomEditor(typeof(WorldStreaming))]
public class WorldStreamingButton : Editor
{

  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();

    WorldStreaming worldStreaming = (WorldStreaming)target;
    if (GUILayout.Button("Create Sector Objects"))
    {
      worldStreaming.CreateSectorObjects();
    }
  }

}
#endif

public class WorldStreaming : MonoBehaviour
{
  [SerializeField] private int gridWidth = 2;
  [SerializeField] private int gridHeight = 2;
  [SerializeField] private float tileWidth = 1000f;
  [SerializeField] private float tileHeight = 1000f;
  [SerializeField] private bool initializeTileWithSize = true;
  [SerializeField] private Material terrainMaterial;
  [SerializeField] private bool initializeTileWithMaterial = true;
  [SerializeField] private int defaultTerrainResolution = 1025;
  [SerializeField] private Transform player;
  [SerializeField] private Transform terrainParent;
  [SerializeField] private Transform gameObjectParent;
  [SerializeField] private float loadDistance = 100f;
  [SerializeField] private float unloadDistance = 150f;

  private Dictionary<Vector2Int, Terrain> loadedTerrains = new Dictionary<Vector2Int, Terrain>();
  private Dictionary<Vector2Int, GameObject> gameObjectSectors = new Dictionary<Vector2Int, GameObject>();

#if UNITY_EDITOR
  public void EditorUpdate()
  {
    LoadClickedSquares();
  }
#endif

  public void CreateSectorObjects()
  {
    Start();
  }

  private void Start()
  {
    // Assemble game object sectors
    for (int x = 0; x < gridWidth; x++)
    {
      for (int y = 0; y < gridHeight; y++)
      {
        Vector2Int tilePos = new Vector2Int(x, y);
        GameObject sector = gameObjectParent.Find($"Sector_{x}_{y}")?.gameObject;
        if (sector == null)
        {
          sector = new GameObject($"Sector_{x}_{y}");
          sector.transform.SetParent(gameObjectParent);
          sector.transform.position = new Vector3(x * tileWidth, 0, y * tileHeight);
        }
        sector.SetActive(false);
        gameObjectSectors[tilePos] = sector;
      }
    }
  }

  private void Update()
  {
    if (Application.isPlaying)
    {
      HandlePlayMode();
    }
  }

  private void HandlePlayMode()
  {
    Vector2Int playerGridPos = new Vector2Int(
      Mathf.FloorToInt(player.position.x / gridWidth),
      Mathf.FloorToInt(player.position.z / gridHeight)
    );

    List<Vector2Int> tilesToLoad = new List<Vector2Int>();
    List<Vector2Int> tilesToUnload = new List<Vector2Int>();

    foreach (var tile in loadedTerrains.Keys)
    {
      float distance = Vector3.Distance(player.position, new Vector3(tile.x * gridWidth, 0, tile.y * gridHeight));
      if (distance > unloadDistance)
      {
        tilesToUnload.Add(tile);
      }
    }

    for (int x = -1; x <= 1; x++)
    {
      for (int y = -1; y <= 1; y++)
      {
        Vector2Int tilePos = new Vector2Int(playerGridPos.x + x, playerGridPos.y + y);
        if (!loadedTerrains.ContainsKey(tilePos))
        {
          float distance = Vector3.Distance(player.position, new Vector3(tilePos.x * gridWidth, 0, tilePos.y * gridHeight));
          if (distance <= loadDistance)
          {
            tilesToLoad.Add(tilePos);
          }
        }
      }
    }

    foreach (var tile in tilesToUnload)
    {
      UnloadTerrain(tile);
    }

    foreach (var tile in tilesToLoad)
    {
      LoadTerrain(tile);
    }
  }

  private void LoadTerrain(Vector2Int tilePos)
  {
    // Implement your terrain loading logic here
    // For example, load terrain data from disk and instantiate it
    string terrainPath = $"Terrains/Terrain_{tilePos.x}_{tilePos.y}";
    TerrainData terrainData = Resources.Load<TerrainData>(terrainPath);
    GameObject terrainPrefab = Resources.Load<GameObject>("TerrainPrefab");
    GameObject terrainGO;
    if (terrainPrefab != null)
    {
      terrainGO = Instantiate(terrainPrefab, Vector3.zero, Quaternion.identity);
      terrainGO.transform.SetParent(terrainParent);
    }
    else
    {
      Debug.LogWarning("Terrain prefab not found in resources.");
      return;
    }
    if (terrainData != null)
    {
      // Set the terrain data
      terrainGO.GetComponent<Terrain>().terrainData = terrainData;
      terrainGO.GetComponent<TerrainCollider>().terrainData = terrainData;
      terrainGO.transform.position = new Vector3(tilePos.x * tileWidth, 0, tilePos.y * tileWidth);
      loadedTerrains[tilePos] = terrainGO.GetComponent<Terrain>();

      if (initializeTileWithMaterial)
      {
        terrainGO.GetComponent<Terrain>().materialTemplate = terrainMaterial;
      }

      terrainGO.GetComponent<Terrain>().allowAutoConnect = true;

    }
    else
    {
      Debug.LogWarning($"Terrain data not found at path: {terrainPath}");
      // Create a new terrain
      terrainGO.GetComponent<Terrain>().terrainData = new TerrainData();
      terrainGO.GetComponent<TerrainCollider>().terrainData = terrainGO.GetComponent<Terrain>().terrainData;
      terrainGO.transform.position = new Vector3(tilePos.x * tileWidth, 0, tilePos.y * tileWidth);
      loadedTerrains[tilePos] = terrainGO.GetComponent<Terrain>();

      // Set terrain resolution
      terrainGO.GetComponent<Terrain>().terrainData.heightmapResolution = defaultTerrainResolution;
      terrainGO.GetComponent<Terrain>().terrainData.alphamapResolution = defaultTerrainResolution;
      terrainGO.GetComponent<Terrain>().terrainData.baseMapResolution = defaultTerrainResolution;

      // Set terrain default values
      if (initializeTileWithSize)
      {
        terrainGO.GetComponent<Terrain>().terrainData.size = new Vector3(tileWidth, tileHeight, tileWidth);
      }

      if (initializeTileWithMaterial)
      {
        terrainGO.GetComponent<Terrain>().materialTemplate = terrainMaterial;
      }

      terrainGO.GetComponent<Terrain>().allowAutoConnect = true;

      // Save the terrain data to disk
#if UNITY_EDITOR
      AssetDatabase.CreateAsset(terrainGO.GetComponent<Terrain>().terrainData, "Assets/Resources/" + terrainPath + ".asset");
      AssetDatabase.SaveAssets();
#else
            Debug.LogWarning("Cannot create asset at runtime. This should only be done in the editor.");
#endif
    }

    // Load sector game object
    if (gameObjectSectors.TryGetValue(tilePos, out GameObject sector))
    {
      sector.SetActive(true);
    }
  }

  private void UnloadTerrain(Vector2Int tilePos)
  {
    if (loadedTerrains.TryGetValue(tilePos, out Terrain terrain))
    {
      if (Application.isPlaying)
      {
        Destroy(terrain.gameObject);
      }
      else
      {
        DestroyImmediate(terrain.gameObject);
      }
      loadedTerrains.Remove(tilePos);
    }

    // Unload sector game object
    if (gameObjectSectors.TryGetValue(tilePos, out GameObject sector))
    {
      sector.SetActive(false);
    }
  }

#if UNITY_EDITOR
  private void LoadClickedSquares()
  {
    bool[] squares = WorldStreamingEditor.GetClickedSquares();

    if (squares == null)
    {
      return;
    }
    //Debug.Log("Loading clicked squares...");
    for (int i = 0; i < squares.Length; i++)
    {
      if (squares[i])
      {
        if (loadedTerrains.ContainsKey(new Vector2Int(i % gridWidth, i / gridWidth)))
        {
          continue;
        }
        // Load the square at position i
        LoadTerrain(new Vector2Int(i % gridWidth, i / gridWidth));
      }
      else
      {
        if (!loadedTerrains.ContainsKey(new Vector2Int(i % gridWidth, i / gridWidth)))
        {
          continue;
        }
        // Unload the square at position i
        UnloadTerrain(new Vector2Int(i % gridWidth, i / gridWidth));
      }
    }
  }
#endif

}