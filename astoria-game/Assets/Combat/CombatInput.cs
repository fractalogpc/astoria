using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles input for the entire combat system. Look here for the initial events that mesh with other scripts.
/// </summary>
public class CombatInput : MonoBehaviour
{
    public static CombatInput Instance;

    [HideInInspector] public UnityEvent FireDown;
    [HideInInspector] public UnityEvent FireUp;
    [HideInInspector] public UnityEvent ADSStart;
    [HideInInspector] public UnityEvent ADSEnd;
    [HideInInspector] public UnityEvent ReloadDown;
    [HideInInspector] public UnityEvent WeaponPrimary;
    [HideInInspector] public UnityEvent WeaponPistol;
    [HideInInspector] public UnityEvent WeaponSpecial;
    
    private void Awake() {
        Singleton();
    }

    // @Elliot switch this to your input system later
    private void Update() {
        if (Input.GetButtonDown("Fire1")) {
            FireDown?.Invoke();
        }
        if (Input.GetButtonUp("Fire1")) {
            FireUp?.Invoke();
        }
        if (Input.GetButtonDown("Fire2")) {
            ADSStart?.Invoke();
        }
        if (Input.GetButtonUp("Fire2")) {
            ADSEnd?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            ReloadDown?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            WeaponPrimary?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            WeaponPistol?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            WeaponSpecial?.Invoke();
        }
    }


    private void Singleton() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }
}
