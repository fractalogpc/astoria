using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveSystemManager : MonoBehaviour
{
	[Header("Dependencies")]
	[SerializeField] private PingManager _pingManager;
	[Header("View")]
	[SerializeField] private ObjectiveSystemView _objectiveSystemView;
	private ObjectiveSystemModel _objectiveSystemModel;

	public void AddObjective(ObjectiveData objectiveData) {
		ObjectiveInstance objectiveInstance = objectiveData.CreateInstance(this);
		_objectiveSystemModel.AddObjective(objectiveInstance);
	}

	public void RemoveObjective(ObjectiveInstance objectiveInstance) {
		_objectiveSystemModel.RemoveObjective(objectiveInstance);
	}

	public void RemoveObjectivesMatching(ObjectiveData objectiveData) {
		for (int i = _objectiveSystemModel.ActiveObjectives.Count - 1; i >= 0; i--) {
			if (_objectiveSystemModel.ActiveObjectives[i].ObjectiveData != objectiveData) return;
			_objectiveSystemModel.RemoveObjective(_objectiveSystemModel.ActiveObjectives[i]);
		}
	}
	
	private void Start() {
		_objectiveSystemModel = new ObjectiveSystemModel();
		_objectiveSystemModel.AttachToEvents(OnObjectiveAdded, OnObjectiveRemoved, OnObjectiveCompleted, OnSelectedObjectiveChanged);
	}

	private void Update() {
		for (int i = _objectiveSystemModel.ActiveObjectives.Count - 1; i >= 0; i--) {
			ObjectiveInstance objective = _objectiveSystemModel.ActiveObjectives[i];
			objective.Tick();
		}
	}

	private void OnDisable() {
		_objectiveSystemModel.DetachFromEvents(OnObjectiveAdded, OnObjectiveRemoved, OnObjectiveCompleted, OnSelectedObjectiveChanged);
	}
	
	private void OnObjectiveAdded(ObjectiveInstance objectiveInstance) {
		
	}
	
	private void OnObjectiveRemoved(ObjectiveInstance objectiveInstance) {
		
	}
	
	private void OnObjectiveCompleted(ObjectiveInstance objectiveInstance) {
		objectiveInstance.Cleanup();
		_objectiveSystemModel.RemoveObjective(objectiveInstance);
	}
	
	private void OnSelectedObjectiveChanged(ObjectiveInstance selectedObjective) {
		
	}
}