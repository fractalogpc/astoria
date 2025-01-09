using System;
using UnityEngine;
using UnityEngine.Events;

public class OnCursorState : MonoBehaviour
{
	public UnityEvent OnMouseUnlocked; 
	public UnityEvent OnMouseLocked;

	private CursorLockMode _lastLockMode;

	private void Start() {
		_lastLockMode = Cursor.lockState;
	}

	private void Update() {
		if (Cursor.lockState == _lastLockMode) return;
		if (Cursor.lockState == CursorLockMode.Locked) {
			OnMouseLocked?.Invoke();
		} else {
			OnMouseUnlocked?.Invoke();
		}
		_lastLockMode = Cursor.lockState;
	}
}
