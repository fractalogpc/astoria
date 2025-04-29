using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PingManager : MonoBehaviour
{
	public static PingManager Instance { get; private set; }
	
	[SerializeField] private RectTransform _pingContainer;
	[SerializeField] private GameObject _pingPrefab;
	[ReadOnly] [SerializeField] private List<RectAtWorldPosition> _pings = new();
	
	private List<Coroutine> _coroutines = new();
	
	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
			return;
		}
		Instance = this;
	}

	public RectAtWorldPosition CreatePingAt(Vector3 position) {
		RectAtWorldPosition ping = Instantiate(_pingPrefab, _pingContainer).GetComponent<RectAtWorldPosition>();
		ping.SetWorldPosition(position);
		_pings.Add(ping);
		return ping;
	}
	
	public RectAtWorldPosition CreatePingAttached(Transform transform) {
		RectAtWorldPosition ping = Instantiate(_pingPrefab, _pingContainer).GetComponent<RectAtWorldPosition>();
		_coroutines.Add(StartCoroutine(AttachPingToTransform(ping, transform)));
		_pings.Add(ping);
		return ping;
	}

	public bool PingExistsAt(Vector3 position, float tolerance = 1f) {
		foreach (RectAtWorldPosition ping in _pings) {
			if (ping.WorldPosition == position) {
				return true;
			}
		}
		return false;
	}
	
	public bool PingExists(RectAtWorldPosition ping) {
		foreach (RectAtWorldPosition existingPing in _pings) {
			if (existingPing == ping) {
				return true;
			}
		}
		return false;
	}
	
	public void ClearPings() {
		foreach (RectAtWorldPosition ping in _pings) {
			Destroy(ping.gameObject);
		}
		_pings.Clear();
	}
	
	public void RemovePing(RectAtWorldPosition ping) {
		if (_pings.Contains(ping)) {
			_pings.Remove(ping);
			Destroy(ping.gameObject);
		}
	}
	
	public void RemovePingAt(Vector3 position, float tolerance = 1f) {
		RectAtWorldPosition pingToRemove = null;
		foreach (RectAtWorldPosition ping in _pings) {
			if (ping.WorldPosition == position) {
				pingToRemove = ping;
				break;
			}
		}

		if (pingToRemove != null) {
			RemovePing(pingToRemove);
		}
	}

	private void OnDisable() {
		foreach (Coroutine coroutine in _coroutines) {	
			StopCoroutine(coroutine);
		}
		_coroutines.Clear();
	}

	private IEnumerator AttachPingToTransform(RectAtWorldPosition ping, Transform transform) {
		while (ping != null && transform != null) {
			ping.SetWorldPosition(transform.position);
			yield return null;
		}
		if (ping != null) {
			Destroy(ping.gameObject);
		}
	}
}