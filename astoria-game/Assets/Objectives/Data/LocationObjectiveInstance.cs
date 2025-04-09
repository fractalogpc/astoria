using System;
using SteamAudio;
using Vector3 = UnityEngine.Vector3;

public class LocationObjectiveInstance : ObjectiveInstance
{
	public LocationObjectiveData ObjectiveData => (LocationObjectiveData)base.ObjectiveData;
	private PingManager _pingManager;
	private RectAtWorldPosition _pingInstance;
	
	public LocationObjectiveInstance(ObjectiveData objectiveData, ObjectiveSystemManager objectiveSystemManager) : base(objectiveData, objectiveSystemManager) {
		_pingManager = objectiveSystemManager.PingManager;
	}
	
	/// <summary>
	/// Called every frame by ObjectiveSystemManager.
	/// </summary>
	public override void Tick() {
		base.Tick();
		if (Vector3.Distance(ObjectiveData.Location, PlayerInstance.Instance.transform.position) < ObjectiveData.CompletionDistance) {
			Complete();
		}
	}

	/// <summary>
	/// Called when the player selects the objective in the  Map Menu.
	/// </summary>
	public override void OnSelect() {
		base.OnSelect();
		if (_pingManager.PingExists(_pingInstance)) return;
		_pingInstance = _pingManager.CreatePingAt(ObjectiveData.Location);
	}

	/// <summary>
	/// Can be called by any script through ObjectiveSystemManager.CompleteObjective().
	/// Can also be called by methods in this class to automatically complete the objective when a condition is met.
	/// </summary>
	public override void Complete() {
		if (_pingInstance != null) {
			_pingManager.RemovePing(_pingInstance);
		}
		base.Complete();
	}
	
	/// <summary>
	/// Should be automatically by the ObjectiveSystemManager through OnObjectiveCompleted after the objective is completed.
	/// </summary>
	public override void Cleanup() {
		base.Cleanup();
	}
}