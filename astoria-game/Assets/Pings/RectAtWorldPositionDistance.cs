using System;
using TMPro;
using UnityEngine;

public class RectAtWorldPositionDistance : MonoBehaviour
{
	[SerializeField] private RectAtWorldPosition _rectAtWorldPosition;
	[SerializeField] private TextMeshProUGUI _distanceText;

	private void Update() {
		float distance = Vector3.Distance(_rectAtWorldPosition.OriginTransform.position, _rectAtWorldPosition.WorldPosition);
		_distanceText.text = $"{distance:N0}m";
	}
}
