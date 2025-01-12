using UnityEngine;

public class BaseToolInstance : ItemInstance
{
	private ToolCore _toolCore;
	private CombatViewmodel _viewmodelManager;
	
	public BaseToolInstance(ItemData itemData) : base(itemData) {
	}

	public void Initialize(ToolCore toolCore, CombatViewmodel viewmodelManager) {
		
	}
	
	
	
	
}
