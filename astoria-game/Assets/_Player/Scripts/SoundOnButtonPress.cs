using UnityEngine;
using FMODUnity;
using UnityEngine.UI;

public class SoundOnButtonPress : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private EventReference _buttonPress;
    [SerializeField] private Button _button;

    private void Start()
    {
        if (_button == null)
        {
            _button = GetComponent<Button>();
            if (_button == null)
            {
                Debug.LogError("Button component not found on this GameObject.");
                return;
            }
        }
        _button.onClick.AddListener(PlayButtonPressSound);
        audioManager = AudioManager.Instance;
    }

    private void PlayButtonPressSound()
    {
        if (audioManager == null)
        {
            Debug.LogError("AudioManager reference is not assigned.");
            return;
        }

        audioManager.PlayOneShot(_buttonPress, transform.position);
    }

}
