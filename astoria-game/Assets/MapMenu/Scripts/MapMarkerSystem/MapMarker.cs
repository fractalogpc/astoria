using UnityEngine;

public class MapMarker
{
	public string Name { get; private set; }
	public Vector3 WorldPosition { get; private set; }
	public Sprite Icon { get; private set; }

	public MapMarker(string name, Vector3 worldPosition, Sprite icon) {
		Name = name;
		WorldPosition = worldPosition;
		Icon = icon;
	}
}