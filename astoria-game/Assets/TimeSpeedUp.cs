using UnityEngine;

public class TimeSpeedUp : MonoBehaviour
{

  public bool deactivate = false;

  void Update()
  {
    if (deactivate) return; // Exit if not enabled
    if (Input.GetKey(KeyCode.F)) // Replace with your condition to speed up time
    {
      if (Input.GetKey(KeyCode.LeftShift)) {
        Time.timeScale = 20f;
      } else {
        Time.timeScale = 5f; // Speed up time when the condition is met
      }
    }
    else {
        Time.timeScale = 1f; // Reset to normal speed when the condition is not met
    }
  }


  public void DisableTimeSpeedUp()
  {
    deactivate = true;
    Time.timeScale = 1f; // Reset to normal speed when disabled
  }
}
