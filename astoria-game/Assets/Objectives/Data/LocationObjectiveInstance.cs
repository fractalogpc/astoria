using System;
using SteamAudio;
using UnityEngine;

/// <summary>
/// An objective instance with an associated location. Does not contain any automatic completion logic.
/// </summary>
public class LocationObjectiveInstance : ObjectiveInstance
{
	public LocationObjectiveData ObjectiveData => (LocationObjectiveData)base.ObjectiveData;
	private PingManager _pingManager;
	private MapMarkerManager _mapMarkerManager;
	private RectAtWorldPosition _pingInstance;
	private MapMarker _associatedMarker;
	private Transform _location;
	
	public LocationObjectiveInstance(ObjectiveData objectiveData, ObjectiveSystemManager objectiveSystemManager, Transform location) : base(objectiveData, objectiveSystemManager) {
		_pingManager = objectiveSystemManager.PingManager;
		_mapMarkerManager = objectiveSystemManager.MapMarkerManager;
		_location = location;
		_mapMarkerManager.AddToMap(new MapMarker(ObjectiveData.Title, _location, ObjectiveData.MapMarkerIcon), _ => OnSelect());
	}
	
	/// <summary>
	/// Called every frame by ObjectiveSystemManager.
	/// </summary>
	public override void Tick() {

	}

	/// <summary>
	/// Called when the player selects the objective in the  Map Menu.
	/// </summary>
	public override void OnSelect() {
		base.OnSelect();
		if (_pingManager.PingExists(_pingInstance)) return;
		_pingInstance = _pingManager.CreatePingAttached(_location);
	}

	/// <summary>
	/// Can be called by any script through ObjectiveSystemManager.CompleteObjective().
	/// Can also be called by methods in this class to automatically complete the objective when a condition is met.
	/// </summary>
	public override void Complete() {
		if (_pingInstance != null) {
			_pingManager.RemovePing(_pingInstance);
		}
		if (_associatedMarker != null) {
			_mapMarkerManager.RemoveFromMap(_associatedMarker);
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