

using System;

public class ObjectiveInstance
{
	public delegate void ObjectiveCompleted(ObjectiveInstance objectiveInstance);
	/// <summary>
	/// When invoked, ObjectiveSystemManager will call Cleanup() and remove this objective from the Active Objectives list.
	/// </summary>
	public event ObjectiveCompleted OnObjectiveCompleted;
	public ObjectiveData ObjectiveData { get; protected set; }
	public ObjectiveSystemManager ObjectiveSystemManager { get; protected set; }
	
	public ObjectiveInstance(ObjectiveData objectiveData, ObjectiveSystemManager objectiveSystemManager){
		ObjectiveData = objectiveData;
		ObjectiveSystemManager = objectiveSystemManager;
	}
	
	/// <summary>
	/// Called every frame by ObjectiveSystemManager.
	/// </summary>
	public virtual void Tick() {
		
	}

	/// <summary>
	/// Called when the player selects the objective in the Map Menu.
	/// </summary>
	public virtual void OnSelect() {
		
	}

	/// <summary>
	/// Can be called by any script through ObjectiveSystemManager.CompleteObjective().
	/// Can also be called by methods in this class to automatically complete the objective when a condition is met.
	/// </summary>
	public virtual void Complete() {
		OnObjectiveCompleted?.Invoke(this);
		if (ObjectiveData.NextObjectiveOptional != null) {
			ObjectiveSystemManager.SelectObjective(ObjectiveSystemManager.AddObjective(ObjectiveData.NextObjectiveOptional));
		}
	}
	
	/// <summary>
	/// Should be automatically by the ObjectiveSystemManager through OnObjectiveCompleted after the objective is completed.
	/// </summary>
	public virtual void Cleanup() {
		OnObjectiveCompleted = null;
	}
}