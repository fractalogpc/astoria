using UnityEngine;

public class TimeSpeedUp : MonoBehaviour
{
  void Update()
  {
    if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.F)) // Replace with your condition to speed up time
    {
        Time.timeScale = 5f;
    }
    else {
        Time.timeScale = 1f; // Reset to normal speed when the condition is not met
    }
  }
}
