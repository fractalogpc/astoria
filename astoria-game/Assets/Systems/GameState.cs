using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

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
    public bool IsLoadingScene => _isLoadingScene;

    public static GameState Instance { get; private set; }

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
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        _isLoadingScene = false;
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
        // Load the loading scene
        yield return SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);

        // Unload the cutscene scene
        yield return SceneManager.UnloadSceneAsync(cutsceneSceneName);

        // Load game scene
        yield return SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);

        Time.timeScale = 1; // Reset time scale to normal

        // Load the cutscene end transition scene
        yield return SceneManager.LoadSceneAsync(cutsceneEndTransitionSceneName, LoadSceneMode.Additive);

        // Unload the loading scene
        yield return SceneManager.UnloadScene(loadingSceneName);

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

        LoadScene(gameSceneName);
        onGameStart.Invoke();
    }

    public void EndGame()
    {
        onGameEnd.Invoke();
        LoadScene(mainMenuSceneName);
        onEnterMenu.Invoke();
    }

}
