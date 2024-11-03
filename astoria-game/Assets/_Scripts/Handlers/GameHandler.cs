using UnityEngine;

public class GameHandler : MonoBehaviour, IOnEnableExecution
{
  public void InitializeOnEnable() {
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }
}
