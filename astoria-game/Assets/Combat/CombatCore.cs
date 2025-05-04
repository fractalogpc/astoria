using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Tightly coupled with CombatViewmodelManager.
///
/// This handles:
/// Holding references to combat scripts
/// Initializing weapons with references to combat scripts
/// Sending inputs to weapons
/// Running coroutines on weapons
/// </summary>
public class CombatCore : InputHandlerBase
{
	public static CombatCore Instance { get; private set; }
	
	public InventoryComponent PlayerInventory;
	public ViewmodelManager ViewmodelManager;
	public Camera PlayerCamera { 
			get {
				if (GameState.Instance != null) {
					return GameState.Instance.GetComponent<Camera>();
        }
				Debug.LogWarning("CombatCore: GameState could not be found! PlayerCamera property will return Camera.main.");
				return Camera.main;
			}
	}
	public FadeElementInOut CrosshairFade;
	public GunInstance CurrentGunInstance { get; private set; }
	
	/// <summary>
	/// Fires when the equipped weapon changes. The parameter is the new weapon.
	/// </summary>
	public UnityEvent<GunInstance> OnEquipWeapon;

	/// <summary>
	/// Fires when the equipped weapon is unequipped.
	/// </summary>
	public UnityEvent OnUnequipWeapon;

	/// <summary>
	/// Fires when the ammo of the equipped weapon changes. The first parameter is the current ammo, the second is the max ammo.
	/// </summary>
	public UnityEvent<int, int> OnAmmoChanged;
	
	public void AttachToInputs(GunInstance instance) {
		CurrentGunInstance = instance;
		OnEquipWeapon?.Invoke(CurrentGunInstance);
		CurrentGunInstance.AmmoChanged += OnInstanceAmmoChanged;
	}

	public void DetachFromInputs() {
		CurrentGunInstance.AmmoChanged -= OnInstanceAmmoChanged;
		CurrentGunInstance = null;
		OnUnequipWeapon?.Invoke();
		// if (CurrentGunInstance == null) return;
		// CurrentGunInstance.Unequip();
		// // StartCoroutine(UnequipWeaponCoroutine());
		// _viewmodelManager.UnsetItem();
		// CurrentGunInstance = null;
		// OnUnequipWeapon?.Invoke();
	}

	protected override void InitializeActionMap() {
		RegisterAction(_inputActions.Player.Attack, ctx => OnFireDown(), () => OnFireUp());
		RegisterAction(_inputActions.Player.Reload, ctx => OnReloadDown(), () => OnReloadUp());
		RegisterAction(_inputActions.Player.AttackSecondary, ctx => OnAimDown(), () => OnAimUp());
		RegisterAction(_inputActions.Player.InspectItem, ctx => OnInspect());
		RegisterAction(_inputActions.Player.SwitchFireMode, ctx => OnSwitchFireMode());
	}

	private void OnDisable() {
		if (CurrentGunInstance == null) return;
		CurrentGunInstance.AmmoChanged -= OnInstanceAmmoChanged;
	}

	private void Awake() {
		if (Instance != null) {
			Debug.LogError("Multiple CombatCore instances detected!");
			Destroy(this);
			return;
		}
		Instance = this;
	}

	private void OnInstanceAmmoChanged(int old, int current) {
		OnAmmoChanged?.Invoke(old, current);
	}

	private void Update() {
		if (CurrentGunInstance == null) return;
		CurrentGunInstance.Tick();
	}

	private void OnFireDown() {
		if (CurrentGunInstance == null) return;
		CurrentGunInstance.OnFireDown();
	}

	private void OnFireUp() {
		if (CurrentGunInstance == null) return;
		CurrentGunInstance.OnFireUp();
	}

	private void OnReloadDown() {
		if (CurrentGunInstance == null) return;
		CurrentGunInstance.OnReloadDown();
	}

	private void OnReloadUp() {
		if (CurrentGunInstance == null) return;
		CurrentGunInstance.OnReloadUp();
	}

	private void OnAimDown() {
		if (CurrentGunInstance == null) return;
		CurrentGunInstance.OnAimDown();
	}

	private void OnAimUp() {
		if (CurrentGunInstance == null) return;
		CurrentGunInstance.OnAimUp();
	}

	private void OnInspect() {
		if (CurrentGunInstance == null) return;
		CurrentGunInstance.OnInspect();
	}

	private void OnSwitchFireMode() {
		if (CurrentGunInstance == null) return;
		CurrentGunInstance.SwitchFireMode();
	}
}