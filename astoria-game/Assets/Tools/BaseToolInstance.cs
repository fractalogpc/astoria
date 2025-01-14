using UnityEngine;

public class BaseToolInstance : ViewmodelItemInstance
{
	protected ToolCore _toolCore;
	protected new ViewmodelManager _viewmodelManager;
	
	public BaseToolInstance(ItemData itemData) : base(itemData) {
	}

	public virtual void Initialize(ToolCore toolCore, ViewmodelManager viewmodelManager) {
		this._toolCore = toolCore;
		this._viewmodelManager = viewmodelManager;
	}
	
	public virtual void OnEquip() {
		
	}
	public virtual void OnUnequip() {
		
	}

	public virtual void OnTick() {
		
	}
	public virtual void OnUseDown() {
		
	}
	public virtual void OnUseUp() {
		
	}
	public virtual void OnUseHold() {
		
	}
	public virtual void OnAltUseDown() {
		
	}
	public virtual void OnAltUseUp() {
		
	}
	public virtual void OnAltUseHold() {
		
	}
}
