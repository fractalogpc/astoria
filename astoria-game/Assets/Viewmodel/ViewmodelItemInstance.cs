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
		_viewmodelManager.SetItemTo(this);
		_viewmodelManager.SetTrigger("Equip");
	}
	
	public override void OnHotbarDeselected() {
		base.OnHotbarDeselected();
		_viewmodelManager.SetTrigger("Unequip");
		_viewmodelManager.UnsetItem();
	}
}