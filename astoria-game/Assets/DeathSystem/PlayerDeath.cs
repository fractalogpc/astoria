using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDeath : MonoBehaviour
{
	public UnityEvent OnDeath;
	[Tooltip("The root transform of the player, used to set the position on respawn.")]
	[SerializeField] private Transform _playerRoot;
	[Tooltip("Used to reset health to max on respawn.")]
	[SerializeField] private HealthManager _healthManager;
	[Tooltip("A black overlay to darken the screen on death.")]
	[SerializeField] private FadeElementInOut _deathDarken;
	[Tooltip("To fade in/out the player UI on death.")]
	[SerializeField] private FadeElementInOut _playerUI;
	[Tooltip("Time it takes to respawn after death, acts as a delay between the darken after death and undarken after respawn.")]
	[SerializeField] private float _respawnTime = 2f;
	
	private Coroutine _deathCoroutine;
	
	public void PlayerDie() {
		if (_deathCoroutine != null) return;
		_deathCoroutine = StartCoroutine(DeathCoroutine());
	}
	
	private IEnumerator DeathCoroutine() {
		SetPlayerInputs(false);
		_playerUI.FadeOut();
		yield return new WaitForSeconds(_playerUI.FadeOutTime);
		_deathDarken.FadeIn();
		yield return new WaitForSeconds(_deathDarken.FadeInTime);
		Transform spawnPoint = GetSpawnpoints()[0];
		_playerRoot.position = spawnPoint.position;
		_playerRoot.rotation = spawnPoint.rotation;
		_healthManager.SetHealthDirect(_healthManager.MaxHealth);
		yield return new WaitForSeconds(_respawnTime);
		_deathDarken.FadeOut();
		yield return new WaitForSeconds(_deathDarken.FadeOutTime);
		_playerUI.FadeIn();
		yield return new WaitForSeconds(_playerUI.FadeInTime);
		SetPlayerInputs(true);
		_deathCoroutine = null;
		OnDeath?.Invoke();
	}

	private void SetPlayerInputs(bool enabled) {
		InputReader.Instance.SwitchInputMap(enabled ? InputMap.Player : InputMap.Cutscene);
	}
	
	private Transform[] GetSpawnpoints() {
		return FindObjectsOfType<PlayerSpawnpoint>()
			.OrderBy(spawnpoint => spawnpoint.Priority)
			.Select(spawnpoint => spawnpoint.transform)
			.ToArray();
	}
}