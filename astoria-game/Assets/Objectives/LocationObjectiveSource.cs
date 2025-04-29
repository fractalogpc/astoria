using System;
using UnityEngine;

public class LocationObjectiveSource : MonoBehaviour
{
	[SerializeField] private LocationObjectiveData _objectiveData;
	[SerializeField] private bool _addObjectiveOnStart;
	private ObjectiveSystemManager _objectiveSystemManager;
	private ObjectiveInstance _objectiveInstance;
	[SerializeField] private bool _completeWithinRadius;
	[SerializeField] private float _radius = 5f;

	private Transform _playerTransform;
	private bool _completed = false;
	
	private void Start() {
		_playerTransform = PlayerInstance.Instance.transform;
		_objectiveSystemManager = PlayerInstance.Instance.GetComponentInChildren<ObjectiveSystemManager>();
		if (_addObjectiveOnStart) {
			StartObjective();
		}
	}

	public void StartObjective() {
		_objectiveInstance = _objectiveData.CreateInstance(_objectiveSystemManager, transform);
		_objectiveSystemManager.AddObjective(_objectiveInstance);
	}
	
	public void CompleteObjective() {
		if (_objectiveInstance == null) {
			Debug.LogError("Objective instance is null. Cannot complete objective.");
			return;
		}
		_objectiveSystemManager.CompleteObjective(_objectiveInstance);
		_completed = true;
	}

	private void Update() {
		if (_completed) return;
		if (!_completeWithinRadius || _objectiveInstance == null) return;
		float distance = Vector3.Distance(_playerTransform.position, transform.position);
		if (distance <= _radius) {
			CompleteObjective();
		}
	}
}