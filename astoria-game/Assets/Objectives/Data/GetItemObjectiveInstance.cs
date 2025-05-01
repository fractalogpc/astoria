using System;
using System.Collections;
using Mirror.BouncyCastle.Asn1;
using SteamAudio;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class GetItemObjectiveInstance : LocationObjectiveInstance
{
	public GetItemObjectiveData ObjectiveData => (GetItemObjectiveData)base.ObjectiveData;
	private InventoryComponent _playerInventory;
	private Coroutine _waitForPlayerInventoryCoroutine;
	
	public GetItemObjectiveInstance(ObjectiveData objectiveData, ObjectiveSystemManager objectiveSystemManager, Transform location) : base(objectiveData, objectiveSystemManager, location) {
		_waitForPlayerInventoryCoroutine = objectiveSystemManager.StartCoroutine(WaitForPlayerInventory());
	}
	
	/// <summary>
	/// Called every frame by ObjectiveSystemManager.
	/// </summary>
	public override void Tick() {
		base.Tick();
		if (ObjectiveData.RequiredItems.ContainedWithin(_playerInventory.GetItems())) {
			Complete();
		}
		Debug.Log("ObjectiveSystem: GetItemObjectiveInstance ticked.");
	}

	/// <summary>
	/// Called when the player selects the objective in the Map Menu.
	/// </summary>
	public override void OnSelect() {
		base.OnSelect();
	}

	/// <summary>
	/// Can be called by any script through ObjectiveSystemManager.CompleteObjective().
	/// Can also be called by methods in this class to automatically complete the objective when a condition is met.
	/// </summary>
	public override void Complete() {
		base.Complete();
	}
	
	/// <summary>
	/// Should be automatically by the ObjectiveSystemManager through OnObjectiveCompleted after the objective is completed.
	/// </summary>
	public override void Cleanup() {
		base.Cleanup();
		if (_waitForPlayerInventoryCoroutine == null) return;
		ObjectiveSystemManager.StopCoroutine(_waitForPlayerInventoryCoroutine);
	}
	
	private IEnumerator WaitForPlayerInventory() {
		while (_playerInventory == null) {
			try {
				_playerInventory = ObjectiveSystemManager.PlayerInventory;
				if (_playerInventory != null) {
					Debug.Log("ObjectiveSystem: PlayerInventory found!");
					break;
				}
			} 
			catch (NullReferenceException e) { }
			yield return null;
		}
	}
}