using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interfaces for classes that require ordered execution of OnEnable.
/// </summary>
public interface IOnEnableExecution
{
  void InitializeOnEnable();
}

/// <summary>
/// Interfaces for classes that require ordered execution of Start.
/// </summary>
public interface IStartExecution
{
  void InitializeStart();
}


/// <summary>
/// Central manager that handles the ordered execution of OnEnable and Start methods for all registered scripts.
/// Ensures that all OnEnable methods run first, followed by all Start methods in a controlled order.
/// </summary>
public class ScriptExecutionManager : PersistentSingleton<ScriptExecutionManager>
{
  // Lists to store scripts that need ordered execution of OnEnable and Start
  private readonly List<IOnEnableExecution> _onEnableScripts = new List<IOnEnableExecution>();
  private readonly List<IStartExecution> _startScripts = new List<IStartExecution>();

  private bool _isOnEnableExecuted = false;

  /// <summary>
  /// Registers a script for ordered execution.
  /// </summary>
  /// <param name="script">Script implementing IOrderedExecution to register.</param>
  public void RegisterScript(IOnEnableExecution script)
  {
    if (!_onEnableScripts.Contains(script))
    {
      _onEnableScripts.Add(script);
    }
  }

  /// <summary>
  /// Registers a script for ordered execution.
  /// </summary>
  /// <param name="script">Script implementing IOrderedExecution to register.</param>
  public void RegisterScript(IStartExecution script)
  {
    if (!_startScripts.Contains(script)) _startScripts.Add(script);
  }

  /// <summary>
  /// Initializes all registered scripts by calling their InitializeOnEnable methods.
  /// </summary>
  public void ExecuteOnEnable()
  {
    foreach (var script in _onEnableScripts)
    {
      script.InitializeOnEnable();
    }
    _isOnEnableExecuted = true;
  }

  /// <summary>
  /// Initializes all registered scripts by calling their InitializeStart methods.
  /// This should only be called after ExecuteOnEnable has run.
  /// </summary>
  public void ExecuteStart()
  {
    if (!_isOnEnableExecuted)
    {
      Debug.LogError("ExecuteOnEnable must be called before ExecuteStart.");
      return;
    }

    foreach (var script in _startScripts)
    {
      script.InitializeStart();
    }
  }

  private void Start()
  {
    ExecuteOnEnable();
    Debug.Log("<color=green>ScriptExecutionManager</color> OnEnable");

    ExecuteStart();
    Debug.Log("<color=green>ScriptExecutionManager</color> Start");
  }

  protected override void Awake()
  {
    base.Awake();
  }

  private void OnDestroy()
  {
    // Clear lists to prevent memory leaks
    _onEnableScripts.Clear();
    _startScripts.Clear();
  }
}
