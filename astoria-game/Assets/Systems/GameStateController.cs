using UnityEngine;
using Mirror;
using System.Collections;

public class GameStateController : NetworkBehaviour
{
  [SerializeField]
  private GameStateHandler gameStateHandler;

  [SerializeField]
  private int totalPlayers = 1; // The total number of players needed to start the game, set in inspector.

  private int playersReady = 0; // Count of players who are ready.

  private void Start()
  {
    if (gameStateHandler == null)
    {
      Debug.LogError("GameStateHandler is not assigned!");
      return;
    }

    // Initially, make sure the game is in the Lobby state.
    if (gameStateHandler.IsInState(GameStateHandler.GameState.None))
    {
      gameStateHandler.ChangeState(GameStateHandler.GameState.Lobby);
    }
  }

  // This method is called whenever a player indicates they are ready
  [Server]
  public void OnPlayerReady()
  {
    playersReady++;

    Debug.Log($"Players ready: {playersReady}/{totalPlayers}");

    if (playersReady >= totalPlayers)
    {
      // All players are ready, enter the countdown state
      EnterCountdownState();
    }
  }

  // This method is called once all players are ready
  [Server]
  private void EnterCountdownState()
  {
    Debug.Log("Entering Countdown state...");
    
    // Change state to Countdown
    gameStateHandler.ChangeState(GameStateHandler.GameState.Countdown);

    // Start a coroutine to handle the countdown logic
    StartCoroutine(CountdownTimer());
  }

  // Coroutine to handle the countdown timer
  private IEnumerator CountdownTimer()
  {
    float countdownTime = 5f; // Set the countdown duration (5 seconds, for example)

    while (countdownTime > 0)
    {
      Debug.Log($"Countdown: {countdownTime}");
      countdownTime -= Time.deltaTime;
      yield return null; // Wait until the next frame
    }

    // After countdown finishes, start the game
    StartGame();
  }

  // This method starts the game by changing the state to Playing
  [Server]
  private void StartGame()
  {
    Debug.Log("Starting the game...");

    // Change state to Playing
    gameStateHandler.ChangeState(GameStateHandler.GameState.Playing);
  }
}
