using System.Collections.Generic;
using UnityEngine;

public interface IOnEnableExecution
{
    void InitializeOnEnable();
}

public interface IStartExecution
{
    void InitializeStart();
}

public class ScriptExecutionManager : PersistentSingleton<ScriptExecutionManager>
{
    private readonly List<IOnEnableExecution> _onEnableScripts = new List<IOnEnableExecution>();
    private readonly List<IStartExecution> _startScripts = new List<IStartExecution>();
    private readonly HashSet<object> _initializedOnEnableScripts = new HashSet<object>();
    private readonly HashSet<object> _initializedStartScripts = new HashSet<object>();

    private bool _isInitialized = false;

    /// <summary>
    /// Registers a script for ordered execution and immediately initializes it if the manager has already run its initial execution.
    /// </summary>
    public void RegisterScript(IOnEnableExecution script)
    {
        if (!_onEnableScripts.Contains(script))
        {
            _onEnableScripts.Add(script);
            // If we've already done our initial execution, immediately initialize this new script
            if (_isInitialized && !_initializedOnEnableScripts.Contains(script))
            {
                script.InitializeOnEnable();
                _initializedOnEnableScripts.Add(script);
            }
        }
    }

    /// <summary>
    /// Registers a script for ordered execution and immediately initializes it if the manager has already run its initial execution.
    /// </summary>
    public void RegisterScript(IStartExecution script)
    {
        if (!_startScripts.Contains(script))
        {
            _startScripts.Add(script);
            // If we've already done our initial execution, immediately initialize this new script
            if (_isInitialized && !_initializedStartScripts.Contains(script))
            {
                script.InitializeStart();
                _initializedStartScripts.Add(script);
            }
        }
    }

    public void ExecuteOnEnable()
    {
        foreach (var script in _onEnableScripts)
        {
            if (!_initializedOnEnableScripts.Contains(script))
            {
                script.InitializeOnEnable();
                _initializedOnEnableScripts.Add(script);
            }
        }
    }

    public void ExecuteStart()
    {
        foreach (var script in _startScripts)
        {
            if (!_initializedStartScripts.Contains(script))
            {
                script.InitializeStart();
                _initializedStartScripts.Add(script);
            }
        }
    }

    private void Start()
    {
        ExecuteOnEnable();
        Debug.Log("<color=green>ScriptExecutionManager</color> OnEnable");

        ExecuteStart();
        Debug.Log("<color=green>ScriptExecutionManager</color> Start");
        
        _isInitialized = true;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnDestroy()
    {
        _onEnableScripts.Clear();
        _startScripts.Clear();
        _initializedOnEnableScripts.Clear();
        _initializedStartScripts.Clear();
    }
}