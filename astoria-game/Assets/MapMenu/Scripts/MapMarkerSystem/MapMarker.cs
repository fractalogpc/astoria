using UnityEngine;

public class MapMarker
{
	public string Name { get; private set; }
	public Vector3 WorldPosition { get; private set; }
	public Sprite Icon { get; private set; }
	public bool Interactable { get; private set; }
	public bool ShowDirection;
	public Vector3 DirectionFacing { get; private set; }

	public MapMarker(string name, Transform attachedTransform, Sprite icon, bool interactable = true) {
		Name = name;
		WorldPosition = attachedTransform.position;
		Icon = icon;
		ShowDirection = false;
		DirectionFacing = Vector2.negativeInfinity;
	}
	
	public MapMarker(string name, Transform attachedTransform, Sprite icon, Vector3 directionFacing, bool interactable = true) {
		Name = name;
		WorldPosition = attachedTransform.position;
		Icon = icon;
		ShowDirection = true;
		DirectionFacing = directionFacing;
	}
	
	public void SetWorldPosition(Vector3 worldPosition) {
		WorldPosition = worldPosition;
	}
	
	public void SetDirectionFacing(Vector3 directionFacing) {
		ShowDirection = true;
		DirectionFacing = directionFacing;
	}
}