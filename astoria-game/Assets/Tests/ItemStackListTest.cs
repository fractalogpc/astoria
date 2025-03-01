using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

public class ItemStackListTest
{
	// A Test behaves as an ordinary method
	[Test]
	public void ConstructorSeparatesStackableItems() {
		List<ItemInstance> items = GetTestItemInstances(GetTestItemData(5), 15);
		ItemStackList stackList = new(items);
		Assert.AreEqual(3, stackList.StackCount);
		Assert.AreEqual(items.Count, stackList.ToItemsList().Count);
	}
	
	[Test]
	public void ConstructorSeparatesNonStackableItems() {
		List<ItemInstance> items = GetTestItemInstances(GetTestItemData(1, false), 15);
		ItemStackList stackList = new(items);
		Assert.AreEqual(items.Count, stackList.StackCount);
	}
	
	[Test]
	public void ToItemsListReturnsCorrectItems() {
		List<ItemInstance> items = GetTestItemInstances(GetTestItemData(5), 15);
		ItemStackList stackList = new(items);
		List<ItemInstance> newItems = stackList.ToItemsList();
		Assert.AreEqual(15, newItems.Count);
		for (int i = 0; i < 15; i++) {
			// Might not return in the same order
			Assert.AreEqual(newItems.Contains(items[i]), true);
		}
	}
	
	[Test]
	public void ToDatasListReturnsCorrectDatas() {
		List<ItemInstance> items = new();
		ItemData data0 = GetTestItemData(1, false);
		ItemData data1 = GetTestItemData(5);
		ItemData data2 = GetTestItemData(10);

		items.AddRange(GetTestItemInstances(data0, 5));
		items.AddRange(GetTestItemInstances(data1, 10));
		items.AddRange(GetTestItemInstances(data2, 15));
		
		ItemStackList stackList = new(items);
		List<ItemData> datas = stackList.ToDatasList();
		// Returns duplicates if multiple stacks are the same type
		// Assert.AreEqual(3, datas.Count);
		Assert.AreEqual(datas.Contains(data0), true);
		Assert.AreEqual(datas.Contains(data1), true);
		Assert.AreEqual(datas.Contains(data2), true);
	}

	[Test]
	public void ReturnsCorrectContainingStack() {
		ItemStackList stackList = new();
		ItemData data0 = GetTestItemData(1, false);
		ItemData data1 = GetTestItemData(5);
		stackList.Add(CreateStackWith(data0, 1));
		stackList.Add(CreateStackWith(data1, 5));
		
		ItemData data2 = GetTestItemData(10);
		ItemStack stack = CreateStackWith(data2, 9);
		ItemInstance instance = GetTestItemInstance(data2);
		stack.Push(instance);
		stackList.Add(stack);
		
		Assert.IsTrue(stackList.Contains(stack));
		Assert.IsTrue(stackList.Contains(instance));
		Assert.AreEqual(stackList.StackContaining(instance), stack);
	}
	
	private ItemStack CreateStackWith(ItemData stackType, int count) {
		ItemStack stack = new(stackType);
		for (int i = 0; i < count; i++) {
			stack.Push(new ItemInstance(stackType));
		}
		return stack;
	}

	private ItemData GetTestItemData(int stackLimit = 10, bool stackable = true) {
		ItemData itemData = ScriptableObject.CreateInstance<ItemData>();
		itemData.ItemName = "Test Item";
		itemData.MaxStackSize = stackLimit;
		itemData.IsStackable = stackable;
		itemData.ItemSize = new Vector2Int(1, 1);
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