using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveSystemModel
{
	public delegate void ObjectiveAdded(ObjectiveInstance objectiveInstance);
	public delegate void ObjectiveRemoved(ObjectiveInstance objectiveInstance);
	public delegate void SelectedObjectiveChanged(ObjectiveInstance selectedObjective);
	public event ObjectiveAdded OnObjectiveAdded;
	public event ObjectiveRemoved OnObjectiveRemoved;
	public event SelectedObjectiveChanged OnSelectedObjectiveChanged;
	
	private ObjectiveInstance.ObjectiveCompleted objectiveCompletedCallback;
	
	public List<ObjectiveInstance> ActiveObjectives { get; private set; } = new();
	public ObjectiveInstance SelectedObjective;
	
	public ObjectiveSystemModel() {
		
	}
	
	public void AttachToEvents(ObjectiveAdded objectiveAdded, ObjectiveRemoved objectiveRemoved, ObjectiveInstance.ObjectiveCompleted objectiveCompleted, SelectedObjectiveChanged selectedObjectiveChanged) {
		OnObjectiveAdded += objectiveAdded;
		OnObjectiveRemoved += objectiveRemoved;
		OnSelectedObjectiveChanged += selectedObjectiveChanged;
		objectiveCompletedCallback = objectiveCompleted;
		foreach (ObjectiveInstance objective in ActiveObjectives) {
			objective.OnObjectiveCompleted += objectiveCompleted;
		}
	}
	
	public void DetachFromEvents(ObjectiveAdded objectiveAdded, ObjectiveRemoved objectiveRemoved, ObjectiveInstance.ObjectiveCompleted objectiveCompleted, SelectedObjectiveChanged selectedObjectiveChanged) {
		OnObjectiveAdded -= objectiveAdded;
		OnObjectiveRemoved -= objectiveRemoved;
		OnSelectedObjectiveChanged -= selectedObjectiveChanged;
		objectiveCompletedCallback = null;
		foreach (ObjectiveInstance objective in ActiveObjectives) {
			objective.OnObjectiveCompleted -= objectiveCompleted;
		}
	}
	
	public void AddObjective(ObjectiveInstance objective) {
		ActiveObjectives.Add(objective);
		OnObjectiveAdded?.Invoke(objective);
		objective.OnObjectiveCompleted += objectiveCompletedCallback;
	}
	
	public void RemoveObjective(ObjectiveInstance objective) {
		if (!ActiveObjectives.Contains(objective)) return;
		ActiveObjectives.Remove(objective);
		objective.OnObjectiveCompleted -= objectiveCompletedCallback;
		OnObjectiveRemoved?.Invoke(objective);
	}
	
	public void ClearObjectives() {
		foreach (ObjectiveInstance objective in ActiveObjectives) {
			objective.OnObjectiveCompleted -= objectiveCompletedCallback;
			OnObjectiveRemoved?.Invoke(objective);
		}
		ActiveObjectives.Clear();
	}
	
	public void SelectObjective(ObjectiveInstance objective) {
		if (!ActiveObjectives.Contains(objective)) return;
		SelectedObjective = objective;
		objective.OnSelect();
		OnSelectedObjectiveChanged?.Invoke(SelectedObjective);
	}
	
	public void DeselectObjective() {
		SelectedObjective = null;
		OnSelectedObjectiveChanged?.Invoke(SelectedObjective);
	}
}