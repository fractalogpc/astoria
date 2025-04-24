using UnityEngine;
using Construction;
using System.Collections;

public class SaveSystem : MonoBehaviour
{

    [System.Serializable]
    private struct SaveSystemData
    {
        public SaveInfo[] saveInfos;
    }

    [System.Serializable]
    public struct SaveInfo
    {
        public string saveName;
        public string saveDate;
        public int saveIndex;
        // Add thumbnail here later
    }

    [System.Serializable]
    public struct SaveGameData
    {
        public bool empty;
        public Vector3 playerPosition;
        public float playerHealth;
        public float playerHunger;
        public float playerThirst;
        public BuildingComponent[] buildingComponents;
    }

    [System.Serializable]
    public struct BuildingComponent {
        public int componentIndex;
        public Vector3 position;
        public Quaternion rotation;
        public float health;
    }

    public static SaveSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        LoadSaveInfo();
    }
    
    public SaveInfo[] GetSaveInfos()
    {
        return saveData.saveInfos;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private int currentSaveIndex = 0;

    private SaveSystemData saveData;

    private SaveGameData FetchSaveGameData()
    {
        // Fetch the current game data to save
        SaveGameData saveGameData = new SaveGameData
        {
            empty = false,
            playerPosition = new Vector3(0, 0, 0), // Replace with actual player position
            playerHealth = 100f, // Replace with actual player health
            playerHunger = 100f, // Replace with actual player hunger
            playerThirst = 100f, // Replace with actual player thirst

            buildingComponents = GetConstructionComponents(),
        };
        return saveGameData;
    }

    private BuildingComponent[] GetConstructionComponents()
    {
        ConstructionComponent[] constructionComponents = FindObjectsByType<ConstructionComponent>(FindObjectsSortMode.None);
        BuildingComponent[] components = new BuildingComponent[constructionComponents.Length];

        for (int i = 0; i < constructionComponents.Length; i++)
        {
            ConstructionComponent cc = constructionComponents[i];
            components[i] = new BuildingComponent
            {
            componentIndex = cc.data.ComponentIndex,
            position = cc.transform.position,
            rotation = cc.transform.rotation,
            health = cc.health // Assuming ConstructionComponent has a Health property
            };
        }

        return components;
    }

    private void LoadGameData(SaveGameData saveGameData)
    {
        Debug.Log("Loading game data: " + JsonUtility.ToJson(saveGameData, true));
        if (saveGameData.empty) {
            Debug.Log("Loaded empty save file, allowing for new game.");
            return;
        }
        
        ConstructionBuilder.Instance.BuildConstruction(saveGameData.buildingComponents);
    }

    public void LoadSaveInfo()
    {
        // Load save data info from json file
        string path = Application.persistentDataPath + "/saveDataInfo.json";

        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveSystemData>(json);

            Debug.Log("Save data loaded from " + path);
        }
        else
        {
            Debug.Log("No save data found, creating new save data.");
            saveData = new SaveSystemData();
            saveData.saveInfos = new SaveInfo[0]; // Initialize with empty array if no file exists

            // Save the new save data to json file
            string json = JsonUtility.ToJson(saveData, true);
            System.IO.File.WriteAllText(path, json);
            Debug.Log("New save data created at " + path);
        }
    }

    public void CreateNewSave()
    {
        SaveInfo newSave = new SaveInfo
        {
            saveName = "Save #" + (saveData.saveInfos.Length + 1),
            saveDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            saveIndex = saveData.saveInfos.Length
        };

        // Add new save to the array
        SaveInfo[] newSaveInfos = new SaveInfo[saveData.saveInfos.Length + 1];
        for (int i = 0; i < saveData.saveInfos.Length; i++)
        {
            newSaveInfos[i] = saveData.saveInfos[i];
        }
        newSaveInfos[newSaveInfos.Length - 1] = newSave;
        saveData.saveInfos = newSaveInfos;

        // Save the new save data to json file
        string path = Application.persistentDataPath + "/saveDataInfo.json";
        string json = JsonUtility.ToJson(saveData, true);
        System.IO.File.WriteAllText(path, json);
        Debug.Log("Save data saved to " + path);

        // Also create a new save game data file
        string savePath = Application.persistentDataPath + "/saves/" + newSave.saveIndex + ".json";
        if (!System.IO.Directory.Exists(Application.persistentDataPath + "/saves/"))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/saves/");
        }
        SaveGameData newSaveGameData = new SaveGameData
        {
            empty = true,
        };
        string saveJson = JsonUtility.ToJson(newSaveGameData, true);
        System.IO.File.WriteAllText(savePath, saveJson);
        Debug.Log("New save game data created at " + savePath);

        Debug.Log("New save created.");
    }

    public void LoadSave(int saveIndex)
    {
        currentSaveIndex = saveIndex;

        StartCoroutine(LoadSaveCoroutine());
    }

    private IEnumerator LoadSaveCoroutine()
    {
        

        string savePath = Application.persistentDataPath + "/saves/" + currentSaveIndex + ".json";
        if (System.IO.File.Exists(savePath))
        {
            string json = System.IO.File.ReadAllText(savePath);
            SaveGameData saveGameData = JsonUtility.FromJson<SaveGameData>(json);

            GameState.Instance.StartGame(saveGameData.empty ? false : true);

            yield return new WaitUntil(() => GameState.Instance.IsLoadingScene == false); // Wait until the loading scene is not loading

            if (!saveGameData.empty) LoadGameData(saveGameData); // Load the game data into the game

            Debug.Log("Loaded save game data from " + savePath);
        }
        else
        {
            Debug.LogError("Save file not found at " + savePath);
        }

        Debug.Log("Save loaded.");
    }

    public void DeleteSave(int saveIndex)
    {
        string savePath = Application.persistentDataPath + "/saves/" + saveIndex + ".json";
        if (System.IO.File.Exists(savePath))
        {
            System.IO.File.Delete(savePath);
            Debug.Log("Deleted save file at " + savePath);
        }

        // Remove the save info from the array
        SaveInfo[] newSaveInfos = new SaveInfo[saveData.saveInfos.Length - 1];
        int j = 0;
        for (int i = 0; i < saveData.saveInfos.Length; i++)
        {
            if (saveData.saveInfos[i].saveIndex != saveIndex)
            {
                newSaveInfos[j] = saveData.saveInfos[i];
                newSaveInfos[j].saveIndex = j; // Update the save index
                j++;
            }
        }
        saveData.saveInfos = newSaveInfos;

        Debug.Log("Save deleted.");
    }

    public void ClearAllSaves()
    {
        // Go through all save files and delete them
        for (int i = 0; i < saveData.saveInfos.Length; i++)
        {
            string savePath = Application.persistentDataPath + "/saves/" + saveData.saveInfos[i].saveIndex + ".json";
            if (System.IO.File.Exists(savePath))
            {
                System.IO.File.Delete(savePath);
                Debug.Log("Deleted save file at " + savePath);
            }
        }
        // Clear the save data info file
        string path = Application.persistentDataPath + "/saveDataInfo.json";
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
            Debug.Log("Deleted save data info file at " + path);
        }

        saveData.saveInfos = new SaveInfo[0]; // Reset the save data info

        Debug.Log("All saves cleared.");
    }

    public void SaveGame()
    {
        string path = Application.persistentDataPath + "/saves/" + currentSaveIndex + ".json";

        if (!System.IO.Directory.Exists(Application.persistentDataPath + "/saves/"))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/saves/");
        }

        SaveGameData saveGameData = FetchSaveGameData(); // Fetch the current game data to save
        string json = JsonUtility.ToJson(saveGameData, true);
        System.IO.File.WriteAllText(path, json);
        Debug.Log("Game saved to " + path);
    }

}