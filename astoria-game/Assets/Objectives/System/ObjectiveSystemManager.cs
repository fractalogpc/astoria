using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveSystemManager : MonoBehaviour
{
	public List<ObjectiveInstance> ActiveObjectives => _objectiveSystemModel.ActiveObjectives;
	public PingManager PingManager => _pingManager;
	
	[Header("Dependencies")]
	[SerializeField] private PingManager _pingManager;
	[Header("View")]
	[SerializeField] private ObjectiveSystemView _objectiveSystemView;
	private ObjectiveSystemModel _objectiveSystemModel;

	public ObjectiveInstance AddObjective(ObjectiveData objectiveData) {
		ObjectiveInstance objectiveInstance = objectiveData.CreateInstance(this);
		_objectiveSystemModel.AddObjective(objectiveInstance);
		return objectiveInstance;
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

	public void SelectObjective(ObjectiveInstance objective) {
		_objectiveSystemModel.SelectObjective(objective);
		_objectiveSystemView.SetGameUITitle(objective.ObjectiveData.Title);
		_objectiveSystemView.SetGameUIDescription(objective.ObjectiveData.Description);
	}

	public void CompleteObjective(ObjectiveInstance instance) {
		instance.Complete();
	}
	
	private void Awake() {
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
		if (_objectiveSystemModel.SelectedObjective == null) {
			SelectObjective(objectiveInstance);
		}
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