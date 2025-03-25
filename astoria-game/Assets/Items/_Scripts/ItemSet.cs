using System;
using System.Collections.Generic;

/// <summary>
/// A class containing an ItemData and a count of that ItemData. Used in ItemSetList.
/// </summary>
[Serializable]
public class ItemSet
{
	/// <summary>
	/// The ItemData identified in this ItemSet.
	/// </summary>
	public ItemData ItemData;
	/// <summary>
	/// The count associated with the ItemData in this ItemSet.
	/// </summary>
	public int ItemCount;

	/// <summary>
	/// Tests if another ItemSet has the same ItemData and ItemCount as this one.
	/// </summary>
	/// <param name="otherSet">The other set to test against.</param>
	/// <returns>Whether the other set matches this set.</returns>
	public bool Equals(ItemSet otherSet) {
		if (ItemData != otherSet.ItemData) return false;
		if (ItemCount != otherSet.ItemCount) return false;
		return true;
	}
}