using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class BaseToolInstance : ViewmodelItemInstance
{
	protected ToolCore _toolCore;
	public BaseToolInstance(ItemData itemData) : base(itemData) {
		_toolCore = ToolCore.Instance;
	}

	public virtual void Initialize() {

	}

	public override void OnHotbarSelected() {
		base.OnHotbarSelected();
		Debug.Log(_toolCore == null);
		_toolCore.AttachToInputs(this);
		Initialize();
		OnEquip();
	}
	public override void OnHotbarDeselected() {
		base.OnHotbarDeselected();
		_toolCore.DetachFromInputs();
		OnUnequip();
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
