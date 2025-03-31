using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class WandererAudio : MonoBehaviour
{
    public bool PlayIntermittentSounds;
    
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private EventReference _intermittent;
    [SerializeField] private EventReference _attack;
    [SerializeField] private EventReference _damaged;
    [SerializeField] private float _intermittentTimeMin, _intermittentTimeMax;
    
    private float waitTime;
    
    private void Start() {
        _audioManager = AudioManager.Instance;
        waitTime = UnityEngine.Random.Range(_intermittentTimeMin, _intermittentTimeMax);
    }

    private void Update() {
        waitTime -= Time.deltaTime;
        if (!(waitTime <= 0)) return;
        waitTime = UnityEngine.Random.Range(_intermittentTimeMin, _intermittentTimeMax);
        if (!PlayIntermittentSounds) return;
        PlayIntermittent();
    }

    public void PlayAttack() {
        _audioManager.PlayOneShot(_attack, transform.position);
    }
    public void PlayDamaged() {
        _audioManager.PlayOneShot(_damaged, transform.position);
    }
    public void PlayIntermittent() {
        _audioManager.PlayOneShot(_intermittent, transform.position);
    }
}
