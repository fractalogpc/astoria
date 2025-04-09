using System;
using UnityEngine;

public class ObjectiveSource : MonoBehaviour
{
	[SerializeField] private ObjectiveData _objectiveData;
	[SerializeField] private bool _addObjectiveOnStart;
	private ObjectiveSystemManager _objectiveSystemManager;
	private ObjectiveInstance _objectiveInstance;

	private void Start() {
		_objectiveSystemManager = PlayerInstance.Instance.GetComponentInChildren<ObjectiveSystemManager>();
		if (_addObjectiveOnStart) {
			StartObjective();
		}
	}

	public void StartObjective() {
		_objectiveInstance = _objectiveSystemManager.AddObjective(_objectiveData);
	}
	
	public void CompleteObjective() {
		if (_objectiveInstance == null) {
			Debug.LogError("Objective instance is null. Cannot complete objective.");
			return;
		}
		_objectiveSystemManager.CompleteObjective(_objectiveInstance);
	}
}