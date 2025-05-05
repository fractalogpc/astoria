using System;
using System.Collections;
using System.Collections.Generic;
using Construction;
using UnityEngine;

public class ObjectiveSystemManager : MonoBehaviour
{
	public List<ObjectiveInstance> ActiveObjectives => _objectiveSystemModel.ActiveObjectives;
	public PingManager PingManager => _pingManager;
	public MapMarkerManager MapMarkerManager => _mapMarkerManager;
	public InventoryComponent PlayerInventory => _playerInventory;

	public ConstructionCore ConstructionCore => _constructionCore;

	[Header("Dependencies")]
	[SerializeField] private MapMarkerManager _mapMarkerManager;
	[SerializeField] private PingManager _pingManager;
	[SerializeField] private InventoryComponent _playerInventory;
	[SerializeField] private ConstructionCore _constructionCore;
	[Header("View")]
	[SerializeField] private ObjectiveSystemView _objectiveSystemView;
	private ObjectiveSystemModel _objectiveSystemModel;
	
	public ObjectiveInstance AddObjective(ObjectiveInstance objectiveInstance) {
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

	private IEnumerator Start() {
		// Wait for the player inventory to be set up
		while (_playerInventory == null) {
			try {
				_playerInventory = PlayerInstance.Instance.GetComponent<InventoryComponent>();
				if (_playerInventory != null) {
					Debug.Log("ObjectiveSystem: PlayerInventory found!");
					break;
				}
			}
			catch (NullReferenceException e) {
				Debug.Log("ObjectiveSystem: Searching for PlayerInventory...");
			}
			yield return null;
		}
	}

	private void Update() {
		for (int i = _objectiveSystemModel.ActiveObjectives.Count - 1; i >= 0; i--) {
			// print("Ticking objective: " + _objectiveSystemModel.ActiveObjectives[i].ObjectiveData.Title);
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