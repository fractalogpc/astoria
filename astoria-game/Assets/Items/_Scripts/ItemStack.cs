using System;
using System.Collections.Generic;

/// <summary>
/// A class containing ItemInstances. 
/// </summary>
[Serializable]
public class ItemStack
{
	/// <summary>
	/// The item data that this ItemStack holds.
	/// </summary>
	public ItemData StackType;
	/// <summary>
	/// The stack of ItemInstances in this ItemStack.
	/// </summary>
	private List<ItemInstance> _items;
	/// <summary>
	/// The count of ItemInstances in this ItemStack.
	/// </summary>
	public int StackCount => _items.Count;

	public List<ItemInstance> Items => _items;
	
	/// <summary>
	/// Constructs an empty ItemStack.
	/// </summary>
	/// <param name="stackType">The item data that the stack is for.</param>
	public ItemStack(ItemData stackType) {
		StackType = stackType;
		_items = new List<ItemInstance>();
	}
	
	/// <summary>
	/// Attempts to push an ItemInstance to the stack. Fails if: the item is not of the same type as the stack, the stack is full, or the item is not stackable.
	/// </summary>
	/// <param name="item">The ItemInstance to push onto the stack.</param>
	/// <returns>Whether the item was successfully pushed onto the stack.</returns>
	public bool Push(ItemInstance item) {
		if (!CouldPush(item)) return false;
		_items.Insert(0, item);
		return true;
	}
	
	/// <summary>
	/// Indicates whether an ItemInstance could be pushed onto the stack. Fails if: the item is not of the same type as the stack, the stack is full, or the item is not stackable.
	/// </summary>
	/// <param name="item">The ItemInstance that would be pushed onto the stack</param>
	/// <returns>Whether the item could be pushed onto the stack.</returns>
	public bool CouldPush(ItemInstance item) {
		if (!item.ItemData == StackType) return false;
		if (StackCount >= StackType.StackLimit) return false;
		return true;
	}
	
	/// <summary>
	/// Pops an ItemInstance from the stack. Returns false if the stack is empty.
	/// </summary>
	/// <param name="item">The item at the top of the stack.</param>
	/// <returns>True if the stack has more items left in it. False otherwise.</returns>
	public bool Pop(out ItemInstance item) {
		if (StackCount == 0) {
			item = null;
			return false;
		}
		item = _items[0]; 
		_items.RemoveAt(0);
		return true;
	}
	
	/// <summary>
	/// Tests if another ItemStack has the same StackType as this one.
	/// </summary>
	/// <param name="otherStack">The other set to test against.</param>
	/// <returns>Whether the other set has the same ItemData as this set.</returns>
	public bool Similar(ItemStack otherStack) {
		return StackType == otherStack.StackType;
	}
}