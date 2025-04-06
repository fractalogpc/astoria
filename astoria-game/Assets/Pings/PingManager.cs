using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PingManager : MonoBehaviour
{
	[SerializeField] private RectTransform _pingContainer;
	[SerializeField] private GameObject _pingPrefab;
	[ReadOnly] [SerializeField] private List<RectAtWorldPosition> _pings = new();

	public void AddPingAt(Vector3 position) {
		RectAtWorldPosition ping = Instantiate(_pingPrefab, _pingContainer).GetComponent<RectAtWorldPosition>();
		ping.SetWorldPosition(position);
		_pings.Add(ping);
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
}