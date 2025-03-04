using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

public class InventoryDataTests
{
	[Test]
	public void HasCorrectDimensionOnCreation() {
		InventoryData inventory = new(5, 5);
		Assert.AreEqual(5, inventory.Width);
		Assert.AreEqual(5, inventory.Height);
		Assert.AreEqual(25, inventory.Containers.Length);
	} 
	[Test]
	public void CanAdd1X1() {
		InventoryData inventory = new(5, 5);
		ItemData itemData = GetTestItemData(1, new Vector2Int(1, 1));
		ItemInstance itemInstance = GetTestItemInstance(itemData);
		ItemStack stack = new(itemData);
		stack.Push(itemInstance);
		GameObject gameObject = new();
		InventoryComponent component = gameObject.AddComponent<InventoryComponent>();
		inventory.TryAddStack(component, stack, out _);
		Assert.IsTrue(inventory.Items.Contains(itemInstance));
		Assert.AreEqual(inventory.Containers[0, 0].HeldStack, stack);
	}
	[Test]
	public void CanAdd1X2() {
		InventoryData inventory = new(5, 5);
		ItemData itemData = GetTestItemData(1, new Vector2Int(1, 2));
		ItemInstance itemInstance = GetTestItemInstance(itemData);
		ItemStack stack = new(itemData);
		stack.Push(itemInstance);
		GameObject gameObject = new();
		InventoryComponent component = gameObject.AddComponent<InventoryComponent>();
		inventory.TryAddStack(component, stack, out _);
		Assert.IsTrue(inventory.Items.Contains(itemInstance));
		Assert.AreEqual(inventory.Containers[0, 0].HeldStack, stack);
		Assert.AreEqual(inventory.Containers[0, 1].HeldStack, stack);
	}
	[Test]
	public void Stacks1X1OnAdd() {
		InventoryData inventory = new(5, 5);
		ItemData itemData = GetTestItemData(4, new Vector2Int(1, 1));
		ItemInstance itemInstance = GetTestItemInstance(itemData);
		ItemStack stack = new(itemData);
		stack.Push(itemInstance);
		GameObject gameObject = new();
		InventoryComponent component = gameObject.AddComponent<InventoryComponent>();
		inventory.TryAddStack(component, stack, out _);
		Assert.IsTrue(inventory.Items.Contains(itemInstance));
		Assert.AreEqual(inventory.Containers[0, 0].HeldStack, stack);
		ItemStack newStack = new(itemData);
		newStack.Push(GetTestItemInstance(itemData));
		newStack.Push(GetTestItemInstance(itemData));
		inventory.TryAddStack(component, newStack, out _);
		Assert.AreEqual(inventory.Items.Count, 3);
		Assert.AreEqual(inventory.Containers[0, 0].HeldStack, stack);
		Assert.AreEqual(inventory.Containers[0, 0].HeldStack.StackCount, 3);
		Assert.IsNull(inventory.Containers[1, 0].HeldStack);
	}
	[Test]
	public void CreatesNewStackForOverflow() {
		InventoryData data = new(5, 5);
		ItemData itemData = GetTestItemData(2, new Vector2Int(1, 1));
		ItemStack stack = new(itemData);
		stack.Push(GetTestItemInstances(itemData, 2));
		InventoryComponent component = CreateInventoryComponent();
		data.TryAddStack(component, stack, out _);
		ItemStack newStack = new(itemData);
		newStack.Push(GetTestItemInstances(itemData, 2));
		data.TryAddStack(component, newStack, out _);
		Assert.AreEqual(data.Items.Count, 4);
		Assert.AreEqual(data.Containers[0, 0].HeldStack.StackCount, 2);
		Assert.AreEqual(data.Containers[1, 0].HeldStack.StackCount, 2);
	}

	[Test]
	public void AddsRotatedStack() {
		InventoryData data = new(5, 5);
		ItemData itemData = GetTestItemData(2, new Vector2Int(1, 2));
		ItemStack stack = CreateStackWith(itemData, 2);
		stack.Rotated = true;
		InventoryComponent component = CreateInventoryComponent();
		data.TryAddStack(component, stack, out _);
		Assert.AreEqual(data.Items.Count, 2);
		Assert.AreEqual(data.Containers[0, 0].HeldStack, stack);
		Assert.AreEqual(data.Containers[0, 1].HeldStack, stack);
	}

	[Test]
	public void StacksRotatedStack() {
		InventoryData data = new(5, 5);
		ItemData itemData = GetTestItemData(4, new Vector2Int(1, 2));
		ItemStack stack = CreateStackWith(itemData, 2);
		stack.Rotated = true;
		InventoryComponent component = CreateInventoryComponent();
		data.TryAddStack(component, stack, out _);
		ItemStack newStack = CreateStackWith(itemData, 2);
		Assert.IsTrue(data.TryAddStack(component, newStack, out _));
		Assert.AreEqual(data.Items.Count, 4);
		Assert.AreEqual(data.Containers[0, 0].HeldStack, stack);
		Assert.AreEqual(data.Containers[0, 1].HeldStack, stack);
		Assert.AreEqual(data.Containers[0, 2].HeldStack, null);
		Assert.IsFalse(data.Stacks.Contains(newStack));	
	}
	
	[Test]
	public void CantAddOutsideBounds() {
		InventoryData data = new(5, 5);
		ItemData itemData = GetTestItemData(1, new Vector2Int(1, 1));
		ItemStack stack = CreateStackWith(itemData, 1);
		InventoryComponent component = CreateInventoryComponent();
		Assert.IsFalse(data.TryAddStackAtPosition(component, stack, new Vector2Int(5, 6)));
	}

	[Test]
	public void CantMergeDifferentStackTypes() {
		InventoryData data = new(5, 5);
		ItemData itemData = GetTestItemData(2, new Vector2Int(1, 2));
		ItemData otherItemData = GetTestItemData(2, new Vector2Int(1, 2));
		ItemStack stack = CreateStackWith(itemData, 1);
		ItemStack otherStack = CreateStackWith(otherItemData, 1);
		InventoryComponent component = CreateInventoryComponent();
		data.TryAddStackAtPosition(component, stack, new Vector2Int(0, 0));
		Assert.IsFalse(data.TryAddStackAtPosition(component, otherStack, new Vector2Int(0, 1)));
	}

	[Test]
	public void CantPlaceStacksWithOverlap() {
		InventoryData data = new(5, 5);
		ItemData itemData = GetTestItemData(2, new Vector2Int(1, 2));
		ItemStack stack = CreateStackWith(itemData, 1);
		ItemStack otherStack = CreateStackWith(itemData, 1);
		otherStack.Rotated = true;
		InventoryComponent component = CreateInventoryComponent();
		data.TryAddStackAtPosition(component, stack, new Vector2Int(0, 0));
		Assert.IsFalse(data.TryAddStackAtPosition(component, otherStack, new Vector2Int(0, 0)));
	}
	
	private InventoryComponent CreateInventoryComponent() {
		GameObject gameObject = new();
		return gameObject.AddComponent<InventoryComponent>();
	}
	
	private ItemStack CreateStackWith(ItemData stackType, int count) {
		ItemStack stack = new(stackType);
		for (int i = 0; i < count; i++) {
			stack.Push(new ItemInstance(stackType));
		}
		return stack;
	}
	
	private ItemData GetTestItemData(int stackLimit, Vector2Int size) {
		ItemData itemData = ScriptableObject.CreateInstance<ItemData>();
		itemData.ItemName = "Test Item";
		itemData.MaxStackSize = stackLimit;
		itemData.ItemSize = size;
		return itemData;
	}
	
	private ItemInstance GetTestItemInstance(ItemData itemData) {
		return new ItemInstance(itemData);
	}

	private List<ItemInstance> GetTestItemInstances(ItemData itemData, int count) {
		List<ItemInstance> items = new();
		for (int i = 0; i < count; i++) {
			items.Add(GetTestItemInstance(itemData));
		}
		return items;
	}
}