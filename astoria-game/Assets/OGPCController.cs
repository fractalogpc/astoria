using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class OGPCController : MonoBehaviour
{
    public static bool DISABLED = true;

    public static OGPCController Instance { get; private set; }

    public TextMeshProUGUI timerText; // Reference to the UI Text component for displaying the timer

    public float timerDuration = 480f; // Time in seconds

    private bool timerIsRunning = false;

    private float timeSinceStart; // Time since timer started in seconds

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        timerText.gameObject.SetActive(false); // Hide the timer UI at the start
        timeSinceStart = 0f; // Initialize time since start
    }

    public UnityEvent OnTimerEnd;
    public UnityEvent OnReset;

    public void StartTimer()
    {
        if (DISABLED) return;
        timerText.gameObject.SetActive(true); // Show the timer UI
        timerIsRunning = true;
    }

    public void StopTimer()
    {
        if (DISABLED) return;
        timerIsRunning = false;
    }

    public void SetTimer(float timeInSeconds)
    {
        if (DISABLED) return;
        timerDuration = timeInSeconds; // Set the timer duration
        timeSinceStart = 0f; // Reset the timer
        // timerIsRunning = false; // Stop the timer
    }

    public void ResetTimer()
    {
        if (DISABLED) return;
        timeSinceStart = 0f; // Reset the timer
    }

    void Update()
    {
        if (DISABLED) return;
        if (timerIsRunning)
        {
            timeSinceStart += Time.unscaledDeltaTime; // Use unscaled time to avoid pausing the timer when the game is paused

            if (timeSinceStart >= timerDuration)
            {
                timerIsRunning = false;
                timeSinceStart = 0f; // Reset the timer
                OnTimerEnd?.Invoke(); // Invoke the event when the timer ends

                DisablePlayerControler(); // Disable player controls when the timer ends
            }
        }

        UpdateTimerUI(); // Update the timer UI
    }

    private void UpdateTimerUI()
    {
        if (DISABLED) return;
        if (timerText != null)
        {
            float remainingTime = timerDuration - timeSinceStart; // Calculate remaining time
            int minutes = Mathf.FloorToInt(remainingTime / 60); // Get minutes
            int seconds = Mathf.FloorToInt(remainingTime % 60); // Get seconds
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Update the UI text
        }
    }

    private void DisablePlayerControler()
    {
        Time.timeScale = 0;
        InputReader.Instance.SwitchInputMap(InputMap.Null);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResetEverything()
    {
        if (DISABLED) return;
        OnReset?.Invoke(); // Invoke the reset event

        timerDuration = 480f;
        StopTimer();
        timeSinceStart = 0f; // Reset the timer
        timerText.gameObject.SetActive(false); // Hide the timer UI
        Time.timeScale = 1f; // Reset time scale to normal
        InputReader.Instance.SwitchInputMap(InputMap.Null); // Switch back to player input map
        // Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        // Cursor.visible = false; // Hide the cursor

    }
}
