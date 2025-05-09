using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
	public List<ItemInstance> Items => _items;
	public int StackCount => _items.Count;
	public bool Rotated;
	public Vector2Int OriginalSize => StackType.ItemSize;
	public Vector2Int Size {
		get {
			if (StackType == null) return Vector2Int.zero;
			return Rotated
				? new Vector2Int(StackType.ItemSize.y, StackType.ItemSize.x)
				: StackType.ItemSize;
		}
	}
	private List<ItemInstance> _items;
	
	/// <summary>
	/// Constructs an empty ItemStack.
	/// </summary>
	/// <param name="stackType">The item data that the stack is for.</param>
	public ItemStack(ItemData stackType) {
		StackType = stackType;
		_items = new List<ItemInstance>();
	}

	/// <summary>
	/// Constructs an ItemStack containing and of the stack type of the given ItemInstance.
	/// </summary>
	/// <param name="item">The item to start the stack with.</param>
	public ItemStack(ItemInstance item) {
		StackType = item.ItemData;
		_items = new List<ItemInstance> {item};
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
	/// Attempts to push an entire list of ItemInstances to the stack. Fails if: the items in the list are not of the same type as the stack, the stack becomes full, or the item is not stackable.
	/// </summary>
	/// <param name="item">The ItemInstance list to push onto the stack.</param>
	/// <returns>Whether the list was successfully pushed onto the stack.</returns>
	public bool Push(List<ItemInstance> items) {
		if (!CouldPush(items)) return false;
		foreach (ItemInstance i in items) {
			_items.Insert(0, i);
		}
		return true;
	}
	
	/// <summary>
	/// Indicates whether an ItemInstance could be pushed onto the stack. Fails if: the item is not of the same type as the stack, the stack is full, or the item is not stackable.
	/// </summary>
	/// <param name="item">The ItemInstance that would be pushed onto the stack</param>
	/// <returns>Whether the item could be pushed onto the stack.</returns>
	public bool CouldPush(ItemInstance item) {
		if (item.ItemData != StackType) return false;
		if (StackCount >= StackType.MaxStackSize) return false;
		return true;
	}
	
	/// <summary>
	/// Indicates whether an ItemStack could be pushed onto the stack. Fails if: the stack is not of the same type as this stack, this stack is full, or the item is not stackable.
	/// </summary>
	/// <param name="other">The ItemStack that would be pushed onto the stack</param>
	/// <returns>Whether the item could be pushed onto the stack.</returns>
	public bool CouldPush(ItemStack other) {
		if (other.StackType != StackType) return false;
		return StackCount + other.StackCount <= StackType.MaxStackSize;
	}
	
	/// <summary>
	/// Indicates whether some ItemInstances could be pushed onto the stack. Fails if: the item is not of the same type as the stack, the stack is full, or the item is not stackable.
	/// </summary>
	/// <param name="items">The ItemInstances that would be pushed onto the stack</param>
	/// <returns>Whether the items could be pushed onto the stack.</returns>
	public bool CouldPush(List<ItemInstance> items) {
		return items.All(CouldPush);
	}
	
	/// <summary>
	/// Pops an ItemInstance from the stack. Returns false if the stack is empty.
	/// </summary>
	/// <param name="item">The item at the top of the stack.</param>
	/// <returns>If the stack has more items left in it.</returns>
	public bool Pop(out ItemInstance item) {
		if (StackCount == 0) {
			item = null;
			return false;
		}
		item = _items[0]; 
		_items.RemoveAt(0);
		return StackCount > 0;
	}

	public bool Remove(ItemInstance itemInstance) {
		int index = _items.IndexOf(itemInstance);
		if (index == -1) return false;
		_items.RemoveAt(index);
		return true;
	}
	
	public bool Contains(ItemInstance item) {
		return _items.Contains(item);
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