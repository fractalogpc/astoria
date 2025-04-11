using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMarkerManager : MonoBehaviour
{
	public List<MapMarker> Markers => _markerUis.ConvertAll(marker => marker.MarkerData);
	[SerializeField] private List<MapMarkerUI> _markerUis = new();
	[SerializeField] private RectTransform _mapRectTransform;
	[SerializeField] private GameObject _markerPrefab;

	[Header("Setup")]
	[SerializeField] private Vector2 _mapSizeWorldUnits;

	[SerializeField] private Vector2 _mapOffsetWorldUnits;
	
	public void AddToMap(MapMarker marker, MapMarkerUI.MarkerSelected markerSelectedCallback = null) {
		GameObject markerObject = Instantiate(_markerPrefab, _mapRectTransform);
		MapMarkerUI markerUI = markerObject.GetComponent<MapMarkerUI>();
		if (markerUI == null) {
			Debug.LogError("MapMarkerUI component not found on marker prefab.");
			return;
		}
		markerUI.Initialize(marker);
		markerUI.RegisterEvent(markerSelectedCallback);
		PositionOnMap(markerUI);
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
			PositionOnMap(markerUI);
		}
	}

	private void PositionOnMap(MapMarkerUI markerUI) {
		RectTransform markerRectTransform = markerUI.MarkerContainer;
		Vector3 worldPosition = markerUI.MarkerData.WorldPosition;
		// Get normalized world position on XZ plane
		Vector3 normalizedWorldPosition = new(
			(worldPosition.x + _mapOffsetWorldUnits.x) / _mapSizeWorldUnits.x,
			0,
			(worldPosition.z + _mapOffsetWorldUnits.y) / _mapSizeWorldUnits.y
		);
		
		// Convert to local position on the map
		markerRectTransform.SetParent(_mapRectTransform, false);
		markerRectTransform.localScale = Vector3.one;
		markerRectTransform.anchorMin = new Vector2(0, 0);
		markerRectTransform.anchorMax = new Vector2(0, 0);
		markerRectTransform.anchoredPosition = new Vector2(normalizedWorldPosition.x, normalizedWorldPosition.z) * _mapRectTransform.rect.size;
		
		markerRectTransform.localRotation = Quaternion.identity;
		if (!markerUI.MarkerData.ShowDirection) return;
		// Set rotation to face the direction
		Vector3 direction = markerUI.MarkerData.DirectionFacing;
		float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
		markerRectTransform.localRotation = Quaternion.Euler(0, 0, angle);
	}
}