using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using Mirror.Examples.Common.Controllers.Tank;

public class GameState : MonoBehaviour
{

    public UnityEvent onCutsceneStart;
    public UnityEvent onCutsceneEnd;
    public UnityEvent onGameStart;
    public UnityEvent onGameEnd;
    public UnityEvent onEnterMenu;

    [SerializeField] private string cutsceneSceneName = "IntroPrototype";
    [SerializeField] private string gameSceneName = "Prototype2";
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string loadingSceneName = "LoadingScene";
    [SerializeField] private string cutsceneEndTransitionSceneName = "CutsceneEndTransition";

    private bool _hasPlayedCutscene = false;
    private bool _isLoadingScene = false;
    public string OGPCSceneName = "OGPCScene";
    public bool IsLoadingScene => _isLoadingScene;
    private bool _isLoadingMainSceneObjects = true;
    public bool IsLoadingMainSceneObjects => _isLoadingMainSceneObjects;

    public bool LoadOGPCStuff = true;

    public static GameState Instance { get; private set; }

    public void StopLoadingGameScene()
    {
        _isLoadingMainSceneObjects = false;
    }

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

        if (LoadOGPCStuff) {
            // Load the OGPC stuff
            SceneManager.LoadScene(OGPCSceneName, LoadSceneMode.Additive);
        }
    }

    private void LoadScene(string sceneName)
    {
        if (_isLoadingScene) return; // Prevent loading if already loading a scene
        if (sceneName == SceneManager.GetActiveScene().name) return; // Prevent loading the same scene
        _isLoadingScene = true;

        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // Load the loading scene first
        yield return SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Single);

        // Unload the current scene
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);

        // Load the new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Unload the loading scene unless the new scene is the game scene
        if (sceneName != gameSceneName)
        {
            yield return SceneManager.UnloadSceneAsync(loadingSceneName);
            // Set the active scene to the new scene
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }

        _isLoadingScene = false;

        if (sceneName == gameSceneName)
        {
            yield return new WaitUntil(() => _isLoadingMainSceneObjects == false); // Wait until the loading scene is not loading
            // Unload loading scene
            yield return SceneManager.UnloadSceneAsync(loadingSceneName);
            // Set the active scene to the new scene
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }
    }

    public void EnterCutscene()
    {
        LoadScene(cutsceneSceneName);
        onCutsceneStart.Invoke();
    }

    public void ExitCutscene()
    {
        onCutsceneEnd.Invoke();
        StartGame();
    }

    public void EndCutsceneTriggered()
    {
        // Addtively load the cutscene end transition scene, additively load the game scene, and then unload the main cutscene scene.
        StartCoroutine(EndCutsceneTriggeredCoroutine());
    }

    private IEnumerator EndCutsceneTriggeredCoroutine()
    {
        Time.timeScale = 1; // Reset time scale to normal

        // I want to do it like this to avoid a loading screen
        // We have to time the transition scene to not reveal the world until the game scene is loaded
        // And the lights + other stuff need to be consistent between the transition and the game scene
        // The proper way to do this is to maintain an "Environment" holder in the transition scene
        // And to disable the cutscene's environment holder when this starts, simultaneously enabling the transition's environment holder
        // Then we can just load the game scene additively and disable the transition's environment holder when the game scene is loaded while enabling the game's environment holder
        
        yield return SceneManager.LoadSceneAsync(cutsceneEndTransitionSceneName, LoadSceneMode.Additive);
        Destroy(GameObject.Find("Systems"));
        EnvironmentHolderManager.InstanceIntroCutscene.gameObject.SetActive(false); // Enable the transition environment holder

        // TODO: This doesn't work. The wind is still too windy which means that switching the TVE Manager in the same frame doesn't work. Fix later.

        yield return SceneManager.UnloadSceneAsync(cutsceneSceneName); // Unload the cutscene scene

        yield return new WaitForSeconds(0.5f);

        yield return SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);
        yield return null;
        EnvironmentHolderManager.InstanceTransition.gameObject.SetActive(false); // Disable the transition environment holder

        onCutsceneEnd.Invoke();
    }

    public void UnloadCutsceneEndTransitionScene()
    {
        // Unload the cutscene end transition scene
        SceneManager.UnloadSceneAsync(cutsceneEndTransitionSceneName);
    }

    public void StartGame(bool hasPlayedCutscene = false)
    {
        if (!hasPlayedCutscene) {
            LoadScene(cutsceneSceneName);
            onCutsceneStart.Invoke();
            _hasPlayedCutscene = true;
            return;
        }

        _isLoadingMainSceneObjects = true; // Set the loading main scene objects to true
        LoadScene(gameSceneName);
        onGameStart.Invoke();
    }

    public void EndGame()
    {
        onGameEnd.Invoke();
        Destroy(GameObject.Find("Systems"));
        Destroy(GameObject.Find("Player"));
        LoadScene(mainMenuSceneName);
        onEnterMenu.Invoke();
    }

}
