

using System;

public class ObjectiveInstance
{
	public delegate void ObjectiveCompleted(ObjectiveInstance objectiveInstance);
	public event ObjectiveCompleted OnObjectiveCompleted;
	public ObjectiveData ObjectiveData { get; private set; }
	public ObjectiveSystemManager ObjectiveSystemManager { get; private set; }
	
	public ObjectiveInstance(ObjectiveData objectiveData, ObjectiveSystemManager objectiveSystemManager){
		ObjectiveData = objectiveData;
		ObjectiveSystemManager = objectiveSystemManager;
	}

	public virtual void Start() {
		
	}

	public virtual void Tick() {
		
	}

	public virtual void OnSelect() {
		
	}
	public virtual void Cleanup() {
		OnObjectiveCompleted = null;
	}
}