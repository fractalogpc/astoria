using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class MapManager : InputHandlerBase
{
	[Header("References")]
	[SerializeField] private RectTransform _mapZoomTransform;
	[SerializeField] private RectTransform _mapPanTransform;
	[Header("Zoom Settings")]
	[SerializeField] private AnimationCurve _zoomAmountVsScale;
	[SerializeField] private float _maxZoom;
	[SerializeField] private float _minZoom;
	[SerializeField] private float _zoomSpeedMultiplier;
	
	private Vector2 _targetScale;
	private Vector2 _targetPosition;
	private bool _middleClickDown;
	
	
	protected override void InitializeActionMap() {
		RegisterAction(_inputActions.GenericUI.ScrollWheel, OnScroll);
		RegisterAction(_inputActions.GenericUI.MiddleClick, MiddleClickDown, MiddleClickUp);
	}

	private void Start() {
		_targetScale = _mapZoomTransform.localScale;
		_targetPosition = _mapZoomTransform.anchoredPosition; 
	}

	private void OnScroll(InputAction.CallbackContext ctx) {
		float scrollDelta = ctx.ReadValue<Vector2>().y;
		if (Mathf.Approximately(scrollDelta, 0f)) return;

		float zoomSpeed = _zoomSpeedMultiplier * _zoomAmountVsScale.Evaluate(_mapZoomTransform.localScale.x / _maxZoom);
		Vector2 mousePosition = Input.mousePosition;

		// Step 1: Get local point relative to RectTransform
		RectTransformUtility.ScreenPointToLocalPointInRectangle(_mapZoomTransform, mousePosition, null, out Vector2 localPointBeforePivotChange);

		// Step 2: Calculate normalized pivot position from local point
		Rect rect = _mapZoomTransform.rect;
		Vector2 newPivot = new Vector2(
			(localPointBeforePivotChange.x - rect.x) / rect.width,
			(localPointBeforePivotChange.y - rect.y) / rect.height
		);

		// Step 3: Adjust anchored position so the content doesn't jump
		Vector2 pivotDelta = newPivot - _mapZoomTransform.pivot;
		Vector2 size = rect.size;
		Vector2 deltaPosition = new Vector2(pivotDelta.x * size.x * _mapZoomTransform.localScale.x, pivotDelta.y * size.y * _mapZoomTransform.localScale.y);
		_targetPosition = _mapZoomTransform.anchoredPosition += deltaPosition;

		// Step 4: Set the new pivot
		_mapZoomTransform.pivot = newPivot;

		// Step 5: Apply zoom with clamping
		float scaleFactor = 1 + zoomSpeed * scrollDelta;
		Vector3 newScale = _mapZoomTransform.localScale * scaleFactor;
		newScale = Vector3.Max(Vector3.one * _minZoom, Vector3.Min(Vector3.one * _maxZoom, newScale));
		_targetScale = newScale;
	}

	private void MiddleClickDown(InputAction.CallbackContext context) {
		_middleClickDown = true;	
	}
	
	private void MiddleClickUp() {
		_middleClickDown = false;
	}
	
	
	// TODO: Make this relative to middle mouse down position instead, deltas are unreliable
	private void WhileMiddleClick() {
		Vector2 mouseDelta = Input.mousePositionDelta;
		if (mouseDelta == Vector2.zero) return;
		
		Vector2 mouseLastFrame = (Vector2)Input.mousePosition - mouseDelta;
		Vector2 mouseThisFrame = Input.mousePosition; 
		
		RectTransformUtility.ScreenPointToLocalPointInRectangle(_mapPanTransform, mouseLastFrame, null, out Vector2 mouseLastLocal);
		RectTransformUtility.ScreenPointToLocalPointInRectangle(_mapPanTransform, mouseThisFrame, null, out Vector2 mouseThisLocal);
		
		Vector2 mouseDeltaLocal = mouseThisLocal - mouseLastLocal;
		_mapPanTransform.anchoredPosition += mouseDeltaLocal;
	}
	
	private void Update() {
		_mapZoomTransform.localScale = Vector3.Lerp(_mapZoomTransform.localScale, _targetScale, Time.deltaTime * _zoomSpeedMultiplier);
		_mapZoomTransform.anchoredPosition = Vector2.Lerp(_mapZoomTransform.anchoredPosition, _targetPosition, Time.deltaTime * _zoomSpeedMultiplier);
		if (_middleClickDown) {
			WhileMiddleClick();
		}
	}
}