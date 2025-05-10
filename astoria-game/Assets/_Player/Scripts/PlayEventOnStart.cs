using System.Collections;
using FMODUnity;
using UnityEngine;

public class PlayEventOnStart : MonoBehaviour
{
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private EventReference _eventToPlay;
    [SerializeField] private bool _attachToPlayer;
    
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
            _audioManager.PlayOneShotAttached(_eventToPlay, PlayerInstance.Instance.gameObject);
        }
        else {
            AudioManager.Instance.PlayOneShotAttached(_eventToPlay,  gameObject);
        }
    }
}
