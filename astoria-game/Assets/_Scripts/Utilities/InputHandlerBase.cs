using UnityEngine;

/// <summary>
/// Abstract class designed to be used as a base for scripts that handle input.
/// </summary>
public abstract class InputHandlerBase : MonoBehaviour
{
  protected PlayerInputActions _inputActions;

  protected virtual void Awake()
  {
    Systems.OnEnableEvent += OnEnableFunction;
  }
  
  protected virtual void OnEnableFunction()
  {
    _inputActions = InputReader.Instance.InputActions;
    SubscribeInputActions();
  }

  protected virtual void OnDisable()
  {
    Systems.OnEnableEvent += OnEnableFunction;
    UnsubscribeInputActions();
  }

  // Abstract methods for derived classes to implement their specific input subscriptions
  protected abstract void SubscribeInputActions();
  protected abstract void UnsubscribeInputActions();
}