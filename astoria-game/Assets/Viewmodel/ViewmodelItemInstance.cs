using UnityEngine;

public class ViewmodelItemInstance : ItemInstance
{
	public new ViewmodelItemData ItemData => (ViewmodelItemData)base.ItemData;
	
	protected ViewmodelManager _viewmodelManager;
	
	public ViewmodelItemInstance(ItemData itemData) : base(itemData) {
		_viewmodelManager = PlayerInstance.Instance.GetComponentInChildren<ViewmodelManager>();
	}

	public override void OnHotbarSelected() {
		base.OnHotbarSelected();
		_viewmodelManager.SetItemTo(ItemData.HeldItemPrefab);
		_viewmodelManager.PlayAnimation(ItemData.EquipAnimation);
	}
	
	public override void OnHotbarDeselected() {
		base.OnHotbarDeselected();
		_viewmodelManager.PlayAnimation(ItemData.UnequipAnimation);
		_viewmodelManager.UnsetItem();
	}
}