using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Mirror.BouncyCastle.Asn1.X509.Qualified;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

/// <summary>
/// Attach this script to RectTransforms to create Inventory UIs.
/// </summary>
[AddComponentMenu("Inventory/Filtered Inventory Component")]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class FilteredInventoryComponent : InventoryComponent
{
	/// <summary>
	/// Whitelist: Only items in the filter will be allowed in the inventory.
	/// Blacklist: Items in the filter will not be allowed in the inventory.
	/// </summary>
	public enum FilterType
	{
		/// <summary>
		/// Only items in the filter will be allowed in the inventory.
		/// </summary>
		Whitelist,
		/// <summary>
		/// Items in the filter will not be allowed in the inventory.
		/// </summary>
		[UsedImplicitly] Blacklist
	}

	
	public FilterType FilterSetting => _filterSetting;
	[Header("Filter Settings")]
	[SerializeField] private FilterType _filterSetting;
	public List<ItemData> Filter => _filter;
	[SerializeField] private List<ItemData> _filter;

	/// <summary>
	/// Sets the filter for this inventory. Note: Items that don't match the filter that are already in the inventory will not be removed.
	/// </summary>
	/// <param name="filter">The list of ItemData to either whitelist or blacklist, depending on FilterSetting.</param>
	public void SetFilter(List<ItemData> filter) {
		_filter ??= new List<ItemData>();
		_filter = filter;
	}
	/// <summary>
	/// Sets the filter setting of the inventory.
	/// </summary>
	/// <param name="filterSetting">The type of filter to use.</param>
	public void SetFilterSetting(FilterType filterSetting) {
		_filterSetting = filterSetting;
	}
	
	public override bool AddStack(ItemStack itemStack) {
		return MatchesFilter(itemStack.StackType) && base.AddStack(itemStack);
	}
	public override bool AddItem(ItemInstance itemInstance) {
		return MatchesFilter(itemInstance.ItemData) && base.AddItem(itemInstance);
	}
	public override bool AddItemByData(ItemData itemData, int count = 1) {
		return MatchesFilter(itemData) && base.AddItemByData(itemData, count);
	}
	public override int AddItemsByData(List<ItemData> items) {
		if (!MatchesFilter(items)) {
			return -1;
		}
		return base.AddItemsByData(items);
	}
	public override bool PlaceStack(ItemStack itemStack, Vector2Int slotIndexBL) {
		return MatchesFilter(itemStack.StackType) && base.PlaceStack(itemStack, slotIndexBL);
	}
	
	public override void HighlightSlotsUnderStack(ItemStack itemStack, Vector2Int slotIndexBL) {
		base.HighlightSlotsUnderStack(itemStack, slotIndexBL);
		if (MatchesFilter(itemStack.StackType)) return;
		List<InventoryContainer> overlappedContainers = GetOverlappedContainers(itemStack, slotIndexBL);
		HighlightContainers(overlappedContainers, ContainerHighlight.Red);
	}
	
	protected virtual bool MatchesFilter(ItemData data) {
		if (FilterSetting == FilterType.Whitelist) {
			return Filter.Contains(data);
		}
		return !Filter.Contains(data);
	}
	private bool MatchesFilter(List<ItemData> datas) {
		return datas.All(data => MatchesFilter(data));
	}
}