using System;
using System.Collections;
using UnityEngine;

public class LinkToPlayerInv : MonoBehaviour
{
	[SerializeField] private InventoryComponent _playerInvReference;

	// This is kinda bad, but hopefully this won't have to wait too long because it'll be placed long after the player is initialized
	private IEnumerator Start() {
		yield return new WaitUntil(() => PlayerInstance.Instance.gameObject.GetComponentInChildren<InventoryComponent>().InventoryData != null);
		_playerInvReference.CreateInvFromInventoryData(PlayerInstance.Instance.gameObject.GetComponentInChildren<InventoryComponent>().InventoryData);
	}
}
