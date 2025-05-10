using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using FMODUnity;

public class FMODAudioSource : MonoBehaviour
{
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private FMODUnity.EventReference _eventReference;

    IEnumerator Start()
    {
        while (_audioManager == null)
        {
            _audioManager = AudioManager.Instance;
            if (_audioManager == null)
            {
                yield return null;
            }
        }
    }
    
    public void Play()
    {
        if (_audioManager == null)
        {
            Debug.LogError("AudioManager reference is not assigned.");
            return;
        }

        _audioManager.PlayOneShot(_eventReference, transform.position);
    }
}
