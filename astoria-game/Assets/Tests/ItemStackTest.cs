using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ItemStackTest
{
	// A Test behaves as an ordinary method
	[Test]
	public void AcceptsSameType() {
		ItemData stackType = GetTestItemData();
		// Use the Assert class to test conditions
		ItemStack stack = new(stackType);
		Assert.AreEqual(stack.CouldPush(GetTestItemInstance(stackType)), true);
	}
	[Test]
	public void RejectsDifferentType() {
		ItemData stackType = GetTestItemData(5);
		ItemData otherType = GetTestItemData(7);
		ItemStack stack = new(stackType);
		Assert.AreEqual(stack.CouldPush(GetTestItemInstance(otherType)), false);
	}
	[Test]
	public void RejectsFullStack() {
		ItemData stackType = GetTestItemData(2);
		ItemStack stack = new(stackType);
		stack.Push(GetTestItemInstance(stackType));
		stack.Push(GetTestItemInstance(stackType));
		Assert.AreEqual(stack.CouldPush(GetTestItemInstance(stackType)), false);
	}
	[Test]
	public void RejectsUnstackable() {
		ItemData stackType = GetTestItemData(10, false);
		ItemStack stack = new(stackType);
		stack.Push(GetTestItemInstance(stackType));
		Assert.AreEqual(stack.CouldPush(GetTestItemInstance(stackType)), false);
	}
	
	// // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
	// // `yield return null;` to skip a frame.
	// [UnityTest]
	// public IEnumerator ItemStackTestWithEnumeratorPasses() {
	// 	// Use the Assert class to test conditions.
	// 	// Use yield to skip a frame.
	// 	yield return null;
	// }

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