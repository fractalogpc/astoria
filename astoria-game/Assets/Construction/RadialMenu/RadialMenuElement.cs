using UnityEngine;


/// <summary>
/// Represents a selection in the RadialMenu class.
/// </summary>
[CreateAssetMenu(fileName = "RadialMenuElement", menuName = "Scriptable Objects/RadialMenuElement")]
public class RadialMenuElement : ScriptableObject
{
	[Tooltip("The selection name. This will show on the Title text element of the RadialMenu.")]
	public string Name;
	[Tooltip("The selection description. This will show on the Description text element of the RadialMenu.")]
	[TextArea(2, 4)] 
	public string Description;
	[Tooltip("The selection icon. This will show on the Icon image element of the RadialMenu. If not set, the null icon will be used.")]
	public Sprite Icon;
	[Tooltip("The index of the element. This is used to determine the order of the elements.")]
	public int Index;
}
