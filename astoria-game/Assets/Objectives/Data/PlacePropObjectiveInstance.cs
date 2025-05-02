

using System;
using Construction;
using Mirror.BouncyCastle.Asn1;
using UnityEngine;

public class PlacePropObjectiveInstance : ObjectiveInstance
{
	public new PlacePropObjectiveData ObjectiveData => (PlacePropObjectiveData)base.ObjectiveData;
	private ConstructionCore _constructionCore;
	
	public PlacePropObjectiveInstance(ObjectiveData objectiveData, ObjectiveSystemManager objectiveSystemManager) : base(objectiveData, objectiveSystemManager) {
		_constructionCore = ObjectiveSystemManager.ConstructionCore;
		_constructionCore.OnObjectPlaced.AddListener(OnPropPlaced);
	}

	private void OnPropPlaced(ConstructionData data) {
		if (data != ObjectiveData.PropConstructionData) return;
		Complete();
	}
	
	/// <summary>
	/// Called every frame by ObjectiveSystemManager.
	/// </summary>
	public override void Tick() {
		base.Tick();
	}

	/// <summary>
	/// Called when the player selects the objective in the Map Menu.
	/// </summary>
	public override void OnSelect() {
		base.OnSelect();
	}

	/// <summary>
	/// Can be called by any script through ObjectiveSystemManager.CompleteObjective().
	/// Can also be called by methods in this class to automatically complete the objective when a condition is met.
	/// </summary>
	public override void Complete() {
		base.Complete();
	}
	
	/// <summary>
	/// Should be automatically by the ObjectiveSystemManager through OnObjectiveCompleted after the objective is completed.
	/// </summary>
	public override void Cleanup() {
		_constructionCore.OnObjectPlaced.RemoveListener(OnPropPlaced);
		base.Cleanup();
	}
}