using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class MapManager : InputHandlerBase
{
	[SerializeField] private RectTransform _mapTransform;
	[SerializeField] private AnimationCurve _zoomIntensity;
	[SerializeField] private float _maxZoom;
	[SerializeField] private float _minZoom;
	[SerializeField] private float _zoomSpeedMultiplier;

	private Vector2 _targetScale;
	private Vector2 _targetPosition;
	
	
	protected override void InitializeActionMap() {
		RegisterAction(_inputActions.GenericUI.ScrollWheel, OnScroll);
	}
	
	private void OnScroll(InputAction.CallbackContext ctx) {
		float scrollDelta = ctx.ReadValue<Vector2>().y;
		if (Mathf.Approximately(scrollDelta, 0f)) return;

		float zoomSpeed = _zoomSpeedMultiplier * _zoomIntensity.Evaluate(_mapTransform.localScale.x / _maxZoom);
		Vector2 mousePosition = Input.mousePosition;

		// Step 1: Get local point relative to RectTransform
		RectTransformUtility.ScreenPointToLocalPointInRectangle(_mapTransform, mousePosition, null, out Vector2 localPointBeforePivotChange);

		// Step 2: Calculate normalized pivot position from local point
		Rect rect = _mapTransform.rect;
		Vector2 newPivot = new Vector2(
			(localPointBeforePivotChange.x - rect.x) / rect.width,
			(localPointBeforePivotChange.y - rect.y) / rect.height
		);

		// Step 3: Adjust anchored position so the content doesn't jump
		Vector2 pivotDelta = newPivot - _mapTransform.pivot;
		Vector2 size = rect.size;
		Vector2 deltaPosition = new Vector2(pivotDelta.x * size.x * _mapTransform.localScale.x, pivotDelta.y * size.y * _mapTransform.localScale.y);
		_mapTransform.anchoredPosition += deltaPosition;

		// Step 4: Set the new pivot
		_mapTransform.pivot = newPivot;

		// Step 5: Apply zoom with clamping
		float scaleFactor = 1 + zoomSpeed * scrollDelta;
		Vector3 newScale = _mapTransform.localScale * scaleFactor;
		newScale = Vector3.Max(Vector3.one * _minZoom, Vector3.Min(Vector3.one * _maxZoom, newScale));
		_mapTransform.localScale = newScale;
	}
}