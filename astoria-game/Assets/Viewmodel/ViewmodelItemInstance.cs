using UnityEngine;

public class ViewmodelItemInstance : ItemInstance
{
	public new ViewmodelItemData ItemData => (ViewmodelItemData)base.ItemData;
	
	protected ViewmodelManager _viewmodelManager;
	
	public ViewmodelItemInstance(ItemData itemData) : base(itemData) {
		
	}

	public override void OnHotbarSelected() {
		base.OnHotbarSelected();
	}
}