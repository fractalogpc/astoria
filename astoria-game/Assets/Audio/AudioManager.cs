using System;
using UnityEngine;
using FMODUnity;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }
}
