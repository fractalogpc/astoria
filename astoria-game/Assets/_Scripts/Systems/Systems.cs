using System;
using System.Collections;

public class Systems : PersistentSingleton<Systems> {
  public static event Action OnEnableEvent;
  public static event Action LateStartEvent;

  // These two functions are useful as it turns out Unity does a really bad job at handling the order of execution of scripts.
  private void OnEnable() => OnEnableEvent?.Invoke();
  
  private void Start() => StartCoroutine(TriggerAfterStart());

  private static IEnumerator TriggerAfterStart() {
    yield return null;
        
    LateStartEvent?.Invoke();
  }
}
