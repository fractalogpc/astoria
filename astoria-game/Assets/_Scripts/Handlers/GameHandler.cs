using UnityEngine;

public class GameHandler : MonoBehaviour
{
  private void Awake() {
    Systems.OnEnableEvent += () => {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    };
  }
}
