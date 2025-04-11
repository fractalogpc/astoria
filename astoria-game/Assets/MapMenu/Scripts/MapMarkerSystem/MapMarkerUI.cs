
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using FMODUnity;
using TMPro;

public class MapMarkerUI : InputHandlerBase, IPointerEnterHandler, IPointerExitHandler
{
	public delegate void MarkerSelected(MapMarkerUI marker);
	public event MarkerSelected OnMarkerSelected;
	public MapMarker MarkerData { get; private set; }

	public RectTransform MarkerContainer => (RectTransform)transform;

	[SerializeField] private Image _icon;
	[SerializeField] private List<FadeElementInOut> _fadeInOnHover;
	[SerializeField] private List<FadeElementInOut> _fadeInOnClick;
	[SerializeField] private TextMeshProUGUI _nameText;
	[SerializeField] private EventReference _clickSound;
	[SerializeField] private EventReference _hoverSound;

	private bool _interactable;
	
	public void Initialize(MapMarker mapMarker)
	{
		_nameText.text = mapMarker.Name;
		_icon.sprite = mapMarker.Icon;
		MarkerData = mapMarker;
		_interactable = mapMarker.Interactable;
	}

	public void RegisterEvent(MarkerSelected markerSelected) {
		if (!_interactable) return;
		OnMarkerSelected += markerSelected;
	}
	public void UnregisterEvent(MarkerSelected markerSelected) {
		if (!_interactable) return;
		OnMarkerSelected -= markerSelected;
	}
	public void Select() {
		if (!_interactable) return;
		OnMarkerSelected?.Invoke(this);
	}
	public void ClearEvents() {
		OnMarkerSelected = null;
	}
	
	private void OnDestroy() {
		ClearEvents();
	}

	private bool _hovering;
	public void OnPointerEnter(PointerEventData eventData) {
		if (!_interactable) return;
		_hovering = true;
		AudioManager.Instance.PlayOneShot(_hoverSound, transform.position);
		foreach (FadeElementInOut fadeElement in _fadeInOnHover) {
			fadeElement.FadeIn();
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		if (!_interactable) return;
		_hovering = false;
		foreach (FadeElementInOut fadeElement in _fadeInOnHover) {
			fadeElement.FadeOut();
		}
	}

	private void OnClickDown() {
		if (!_interactable) return;
		AudioManager.Instance.PlayOneShot(_clickSound, transform.position);
	}
	
	private void OnClickUp() {
		if (!_interactable) return;
		if (!_hovering) return; 
		Select();
	}
	
	protected override void InitializeActionMap() {
		RegisterAction(_inputActions.GenericUI.Click, _ => OnClickDown(), OnClickUp);
	}
}