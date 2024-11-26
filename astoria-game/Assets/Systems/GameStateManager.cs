using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameStateHandler : NetworkBehaviour
{
  public enum GameState
  {
    None,
    Lobby,
    Countdown,
    Playing,
    GameOver
  }

  [SyncVar(hook = nameof(OnGameStateChanged))]
  private GameState currentState = GameState.None;

  private readonly Dictionary<GameState, Action> onStateEnterActions = new Dictionary<GameState, Action>();
  private readonly Dictionary<GameState, Action> onStateExitActions = new Dictionary<GameState, Action>();

  public GameState CurrentState => currentState;

  public override void OnStartServer()
  {
    // Set initial state
    ChangeState(GameState.Lobby);
  }

  /// <summary>
  /// Adds an action to be invoked when a specific state is entered.
  /// </summary>
  public void AddOnStateEnter(GameState state, Action action)
  {
    if (!onStateEnterActions.ContainsKey(state))
      onStateEnterActions[state] = action;
    else
      onStateEnterActions[state] += action;
  }

  /// <summary>
  /// Adds an action to be invoked when a specific state is exited.
  /// </summary>
  public void AddOnStateExit(GameState state, Action action)
  {
    if (!onStateExitActions.ContainsKey(state))
      onStateExitActions[state] = action;
    else
      onStateExitActions[state] += action;
  }

  /// <summary>
  /// Changes the game state. Only the server can invoke this.
  /// </summary>
  [Server]
  public void ChangeState(GameState newState)
  {
    if (currentState == newState)
      return;

    // Invoke OnStateExit for the current state
    if (onStateExitActions.ContainsKey(currentState))
      onStateExitActions[currentState]?.Invoke();

    // Change the state
    currentState = newState;

    // Invoke OnStateEnter for the new state
    if (onStateEnterActions.ContainsKey(newState))
      onStateEnterActions[newState]?.Invoke();
  }

  /// <summary>
  /// Called whenever the SyncVar `currentState` changes.
  /// </summary>
  private void OnGameStateChanged(GameState oldState, GameState newState)
  {
    if (isServer)
      return; // Server already handles state logic

    // Clients can perform local actions based on state change
    HandleClientStateChange(newState);
  }

  /// <summary>
  /// Logic for handling state changes specific to the client.
  /// </summary>
  private void HandleClientStateChange(GameState state)
  {
    Debug.Log($"Client: State changed to {state}");

    // Add client-specific logic for each state if needed
    switch (state)
    {
      case GameState.Lobby:
        // Handle lobby-specific logic for clients
        break;
      case GameState.Countdown:
        // Handle countdown-specific logic for clients
        break;
      case GameState.Playing:
        // Handle gameplay-specific logic for clients
        break;
      case GameState.GameOver:
        // Handle game over-specific logic for clients
        break;
    }
  }

  /// <summary>
  /// Utility function for getting the current game state.
  /// </summary>
  public bool IsInState(GameState state)
  {
    return currentState == state;
  }
}
