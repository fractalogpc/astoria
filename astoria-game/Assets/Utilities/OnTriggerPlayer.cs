using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class OnTriggerPlayer : MonoBehaviour
{
    public UnityEvent OnEnter;
    public UnityEvent OnExit;
    public UnityEvent OnStay;

    private void OnValidate() {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        OnEnter?.Invoke();
        Debug.Log("OnTriggerEnter");
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        OnExit?.Invoke();
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        OnStay?.Invoke();
    }
}
