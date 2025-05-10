using System.Collections;
using FMODUnity;
using FMOD.Studio;
using UnityEngine;

public class PlayEventOnStart : MonoBehaviour
{
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private EventReference _eventToPlay;
    [SerializeField] private bool _attachToPlayer;

    private EventInstance _sound;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        while (_audioManager == null) {
            _audioManager = AudioManager.Instance;
            yield return null;
        }
        if (_attachToPlayer) {
            // Wait for the player to be instantiated
            while (PlayerInstance.Instance == null) {
                yield return null;
            }
            _sound = _audioManager.PlayOneShotAttached(_eventToPlay, PlayerInstance.Instance.gameObject);
        }
        else {
            _sound = AudioManager.Instance.PlayOneShotAttached(_eventToPlay,  gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_sound.isValid()) {
            _sound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
