using System;
using UnityEngine;

public class PlayerMarker : MonoBehaviour
{
	[SerializeField] private Transform _playerTransform;
	[SerializeField] private MapMarkerManager _markerManager;
	[SerializeField] private Sprite _markerIcon;
	private MapMarker _marker;
	
	private void Start() {
		_marker = new MapMarker("Player", _playerTransform, _markerIcon, _playerTransform.forward, false);
		_markerManager.AddToMap(_marker);
	}
	
	private void Update() {
		if (_marker == null) return;
		_marker.SetWorldPosition(_playerTransform.position);
		_marker.SetDirectionFacing(_playerTransform.forward);
	}
}