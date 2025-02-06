using UnityEngine;

// public class GameStateController : NetworkBehaviour
public class GameStateController : MonoBehaviour
{
  /*
  [SerializeField]
  private GameStateHandler gameStateHandler;

  // Example condition: Track if all players are ready
  private int playersReady = 0;
  private int totalPlayers = 1; // Adjust based on your player count

  private void Start()
  {
    if (gameStateHandler == null)
    {
      Debug.LogError("GameStateHandler is not assigned!");
    }
  }

  [Server]
  public void OnPlayerReady()
  {
    playersReady++;

    Debug.Log($"Players ready: {playersReady}/{totalPlayers}");

    if (playersReady >= totalPlayers)
    {
      // All players are ready, start the game
      StartGame();
    }
  }

  [Server]
  private void StartGame()
  {
    if (gameStateHandler == null)
    {
      Debug.LogError("GameStateHandler not assigned on server!");
      return;
    }

    Debug.Log("Changing game state to Playing...");
    gameStateHandler.ChangeState(GameStateHandler.GameState.Playing);
  }
  */
}
