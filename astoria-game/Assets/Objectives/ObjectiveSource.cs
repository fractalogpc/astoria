using System;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveSource : MonoBehaviour
{
	public UnityEvent OnObjectiveCompleted;
	public bool Completed => _completed;
	[SerializeField] private ObjectiveData _objectiveData;
	[SerializeField] private bool _addObjectiveOnStart;
	private ObjectiveSystemManager _objectiveSystemManager;
	private ObjectiveInstance _objectiveInstance;
	private bool _completed;
	
	private void Start() {
		_objectiveSystemManager = PlayerInstance.Instance.GetComponentInChildren<ObjectiveSystemManager>();
		if (_addObjectiveOnStart) {
			StartObjective();
		}
	}

	public void StartObjective() {
		_completed = false;
		_objectiveInstance = _objectiveData.CreateInstance(_objectiveSystemManager);
		_objectiveInstance.OnObjectiveCompleted += _ => ObjectiveCompleted();
		_objectiveSystemManager.AddObjective(_objectiveInstance);
		_objectiveSystemManager.SelectObjective(_objectiveInstance);
	}
	
	public void ManualCompleteObjective() {
		if (_objectiveInstance == null) {
			Debug.LogError("Objective instance is null. Cannot complete objective.");
			return;
		}
		_objectiveSystemManager.CompleteObjective(_objectiveInstance);
		ObjectiveCompleted();
	}

	private void ObjectiveCompleted() {
		_completed = true;
		OnObjectiveCompleted?.Invoke();
	}
}