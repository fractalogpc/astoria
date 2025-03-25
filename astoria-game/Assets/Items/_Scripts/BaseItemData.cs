using UnityEngine;

/// <summary>
/// This class exists to require that all item data classes have a CreateItem method.
/// </summary>
public abstract class BaseItemData : ScriptableObject
{
	/// <summary>
	/// This method is called by the inventory system to create items.
	/// Ensure that the return type is the associated ItemInstance type for this item data.
	/// </summary>
	/// <returns>An instance of an item.</returns>
	public abstract ItemInstance CreateItem();
}