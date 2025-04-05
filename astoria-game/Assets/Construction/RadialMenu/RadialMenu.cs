using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Mirror;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RadialMenu : InputHandlerBase
{
	public bool Enabled;

	public delegate void RadialElementParam(RadialMenuElement element);
	/// <summary>
	/// Invoked when the user inputs the Click action. Will pass the selected element.
	/// </summary>
	public event RadialElementParam OnElementSelected;
	/// <summary>
	/// Invoked when the user hovers over a new element. Will pass the hovered element.
	/// </summary>
	public event RadialElementParam OnElementHovered;
	public List<RadialMenuElement> RadialMenuElements => _radialMenuElements;
	
	[Header("References")]
	[Tooltip("The origin of the radial menu. Used to determine mouse angle.")]
	[FormerlySerializedAs("_radialBackground")]
	[SerializeField] private RectTransform _radialOrigin;
	[Tooltip("The element that will be rotated to show the selected element.")]
	[SerializeField] private RectTransform _selectorElement;
	[Tooltip("The prefab used to create the elements.")]
	[ReadOnly][SerializeField] private GameObject _elementPrefab;
	[Tooltip("The parent object that will hold the elements.")]
	[SerializeField] private Transform _elementsParent;
	[Tooltip("The text object that will show the name of the selected element.")]
	[SerializeField] private TextMeshProUGUI _nameText;
	[Tooltip("The text object that will show the description of the selected element.")]
	[SerializeField] private TextMeshProUGUI _descriptionText;
	[Header("Settings")]
	[Tooltip("The elements that will be displayed in the radial menu.")]
	[SerializeField] private List<RadialMenuElement> _radialMenuElements = new();
	[Tooltip("The speed the selector will rotate to the selected element.")]
	[SerializeField] private float _selectorSnappiness = 10f;
	[Tooltip("The angle offset of the selector. Use this to align the selector if it's not facing the mouse.")]
	[FormerlySerializedAs("_selectorAngleOffset")] 
	[SerializeField] private float _selectorVisualOffset = 90f;
	[Tooltip("The elements will be placed this distance around the radial origin.")]
	[SerializeField] private float _elementCenterDist = 420f;
	[Tooltip("The distance from the radial origin that the selector will not be active within. If the mouse is within this distance, the selector will not be shown.")]
	[SerializeField] private float _centerDeadZoneRadius;
	
	private float _targetAngle;
	private RadialMenuElement _lastSelected;

	public void ResetLastSelected() {
		_lastSelected = null;
	}
	
	protected override void InitializeActionMap() {
		RegisterAction(_inputActions.GenericUI.Click, ctx => {
			if (!Enabled) return;
			if (!ctx.performed) return;
			RadialMenuElement selected = GetSelectedElement();
			OnElementSelected?.Invoke(selected);
		});
	}

	private void CreateElements() {
		for (int index = 0; index < _radialMenuElements.Count; index++) {
			
			RadialMenuElement element = _radialMenuElements[index];
			GameObject newElement = Instantiate(_elementPrefab, _elementsParent);
			RectTransform rectTransform = newElement.GetComponent<RectTransform>();
			if (rectTransform == null) {
				Debug.LogError("RadialMenu: Element prefab does not have a RectTransform component. Cannot CreateElements.");
				return;
			}

			float angle = index * (360f / _radialMenuElements.Count);
			rectTransform.localPosition = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * _elementCenterDist, Mathf.Sin(angle * Mathf.Deg2Rad) * _elementCenterDist, 0);
			newElement.GetComponentInChildren<Image>().sprite = element.Icon == null ? Resources.Load<Sprite>("DefaultItemAssets/NullImage") : element.Icon;
			newElement.GetComponentInChildren<Image>().preserveAspect = true;
		}
	}

	private void Start() {
		CreateElements();
	}

	private void Update() {
		if (!Enabled) return;
		if (GetMouseDistFromRect(_radialOrigin) < _centerDeadZoneRadius) {
			_selectorElement.gameObject.SetActive(false);
			return;
		}
		_selectorElement.gameObject.SetActive(true);

		RadialMenuElement selected = GetSelectedElement();
		float anglePerSlot = 360f / _radialMenuElements.Count;
		_targetAngle = GetMouseAngleIndex() * anglePerSlot;
		float setAngle = _targetAngle; //Mathf.LerpAngle(_selectorElement.rotation.eulerAngles.z - _selectorVisualOffset, _targetAngle, Time.deltaTime * _selectorSnappiness);
		_selectorElement.rotation = Quaternion.Euler(0, 0, setAngle + _selectorVisualOffset);
	}

	private int GetMouseAngleIndex() {
		float mouseAngle = GetMouseAngleFromRect(_radialOrigin);
		if (mouseAngle < 0) mouseAngle += 360f; // convert to 0-360 range
		float anglePerSlot = 360f / _radialMenuElements.Count;
		mouseAngle += anglePerSlot / 2; // offset to edge of slot
		return Mathf.FloorToInt(mouseAngle / anglePerSlot) % _radialMenuElements.Count;
	}
	
	private RadialMenuElement GetSelectedElement()
	{
		int selectedIndex = GetMouseAngleIndex();
		if (_lastSelected != _radialMenuElements[selectedIndex])
		{
			OnElementHovered?.Invoke(_radialMenuElements[selectedIndex]);
			_nameText.text = _radialMenuElements[selectedIndex].Name;
			_descriptionText.text = _radialMenuElements[selectedIndex].Description;
		}
		_lastSelected = _radialMenuElements[selectedIndex];
		return _radialMenuElements[selectedIndex];
	}
	
	private float GetMouseDistFromRect(RectTransform rect)
	{
		Vector2 mousePos = Input.mousePosition;
		Vector2 rectPos = rect.position;
		float dist = Vector2.Distance(mousePos, rectPos);
		return dist;
	}
	private float GetMouseAngleFromRect(RectTransform rect)
	{
		Vector2 mousePos = Input.mousePosition;
		Vector2 rectPos = rect.position;
		Vector2 direction = mousePos - rectPos;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		return angle;
	}
}
