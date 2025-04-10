using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMarkerManager : MonoBehaviour
{
	public List<MapMarker> Markers => _markerUis.ConvertAll(marker => marker.MarkerData);
	[SerializeField] private List<MapMarkerUI> _markerUis = new();
	[SerializeField] private Transform _mapRectTransform;
	[SerializeField] private GameObject _markerPrefab;

	[Header("Setup")]
	[SerializeField] private Vector2 _mapSizeWorldUnits;

	[SerializeField] private Vector2 _mapOffsetWorldUnits;
	
	public void AddToMap(MapMarker marker, MapMarkerUI.MarkerSelected markerSelectedCallback) {
		GameObject markerObject = Instantiate(_markerPrefab, _mapRectTransform);
		MapMarkerUI markerUI = markerObject.GetComponent<MapMarkerUI>();
		if (markerUI == null) {
			Debug.LogError("MapMarkerUI component not found on marker prefab.");
			return;
		}
		markerUI.Initialize(marker);
		markerUI.RegisterEvent(markerSelectedCallback);
		PositionOnMap(markerUI.MarkerContainer, markerUI.MarkerData.WorldPosition);
		_markerUis.Add(markerUI);
	}
	
	public void RemoveFromMap(MapMarker marker) {
		for (int i = _markerUis.Count - 1; i >= 0; i--) {
			if (_markerUis[i].MarkerData != marker) continue;
			// Event unregister in OnDestroy()
			Destroy(_markerUis[i].gameObject);
			_markerUis.Remove(_markerUis[i]);
			break;
		}
	}

	private void Update() {
		foreach (MapMarkerUI markerUI in _markerUis) {
			// Find the marker object in the list
			PositionOnMap(markerUI.MarkerContainer, markerUI.MarkerData.WorldPosition);
		}
	}

	private void PositionOnMap(RectTransform markerRectTransform, Vector3 worldPosition) {
		// Get normalized world position on XZ plane
		Vector3 normalizedWorldPosition = new(
			(worldPosition.x + _mapOffsetWorldUnits.x) / _mapSizeWorldUnits.x,
			0,
			(worldPosition.z + _mapOffsetWorldUnits.y) / _mapSizeWorldUnits.y
		);
		
		// Convert to local position on the map
		markerRectTransform.SetParent(_mapRectTransform, false);
		markerRectTransform.anchoredPosition = new Vector2(normalizedWorldPosition.x, normalizedWorldPosition.z);
	}
}