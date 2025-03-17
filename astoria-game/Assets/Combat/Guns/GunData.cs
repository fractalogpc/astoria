using System;
using UnityEngine;

// TODO: Make a custom editor that changes the exposed fields based on the weapon type
[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ViewmodelItemData
{
	[Header("Ammo Item")]
	public ItemData AmmoItem;

	[Header("Animations")]
	public AnimationClip FireAnimation;
	public AnimationClip ReloadEmptyAnimation;
	public AnimationClip ReloadPartialAnimation;
	
	[Header("Accuracy Settings")]
	public AccuracySettings AccuracySetting;
	public RecoilSettings RecoilSettings;
	
	[Header("Fire Settings")]
	[
		Tooltip(
			"Fire types concatenate to form the final fire type.\n" +
			"For example, SemiBurstFull is a weapon that can fire in semi-auto, burst, and full-auto.\n" +
			"Semi, Burst, and Full are self-explanatory. Shotgun spawns multiple projectiles at once."
		)
	]
	public FireCombinations FireCombination;
	public SemiAutoSettings SemiAutoSetting;
	public BurstSettings BurstSetting;
	public FullAutoSettings FullAutoSetting;
	public ShotgunSettings ShotgunSetting;
	
	[Header("Reload Settings")]
	[
		Tooltip(
			"MagazineClosedBolt: Reloads by resetting held ammo to max. Will save one bullet in the chamber.\n" + 
			"MagazineOpenBolt: Reloads by resetting held ammo to max. Will not save one bullet in the chamber.\n" + 
			"InternalClosedBolt: Reloads by adding ammo one by one until full. Can cancel reload and keep ammo. Will save one bullet in the chamber.\n" + 
			"InternalOpenBolt: Reloads by adding ammo one by one until full. Can cancel reload and keep ammo. Will not save one bullet in the chamber."
		)
	]
	public ReloadTypes ReloadType;
	public MagazineSettings MagazineSetting;
	public InternalSettings InternalSetting;
	public ChamberSettings ChamberSetting;
	
	[Header("Bullet Data")]
	public float Damage = 30f;
	[Header("Projectile Settings")]
	public int SamplesPerFixedUpdate = 5;
	public float InitialVelocityMS = 400f;
	public float BulletMassKg = 0.007f;
	public float BulletDiameterM = 0.009f;
	public float AirDensityKgPerM = 1.1f;
	public float DragCoefficient = 0.149f;
	
	public override ItemInstance CreateItem() {
		return new GunInstance(this);
	}
}

[System.Serializable]
public struct RecoilSettings
{
	public AnimationCurve RecoilCurve;
	public float MeanUpwardsRecoil;
	public float MeanHorizontalRecoil;
	public float MeanBackwardsRecoil;
	public float UpwardsRecoilVariation;
	public float HorizontalRecoilVariation;
	public float BackwardsRecoilVariation;
	public float MeanRecoilTime;
	public float RecoilTimeVariation;
	public float NoiseMagnitude;
	public float NoiseSpeed;
	public float MainTransferToViewmodel;

}


/// <summary>
/// Firing types. WeaponInstance uses this to switch logic.
/// </summary>
public enum FireCombinations
{
	Semi,
	SemiBurst,
	SemiFull,
	SemiBurstFull,
	ShotgunSemi,
	ShotgunSemiBurst,
	ShotgunSemiFull,
	ShotgunSemiBurstFull,
}

/// <summary>
/// Reload types. WeaponInstance uses this to switch logic.
/// </summary>
public enum ReloadTypes
{
	MagazineClosedBolt,
	MagazineOpenBolt,
	InternalClosedBolt,
	InternalOpenBolt,
}

// TODO: Set reasonable defaults for all fields below

[Serializable]
public struct SemiAutoSettings
{
	public float CycleTime;
}

[Serializable]
public struct BurstSettings
{
	public int ShotsPerBurst;
	public float RoundsPerMinute;
	public float CycleTime;
}

[Serializable]
public struct FullAutoSettings
{
	public float RoundsPerMinute;
}

[Serializable]
public struct ShotgunSettings
{
	public int PelletsPerShot;
}

[Serializable]
public struct AccuracySettings
{
	[Tooltip("The distance where a projectile fired by the weapon, when aimed down sights, will land in a circle of diameter PatternSpread.")]
	public float EffectiveRange;
	public float PatternSpread;
}
[Serializable]
public struct MagazineSettings
{
	public int MagazineCapacity;
	public float ReloadTime;
}
[Serializable]
public struct InternalSettings
{
	public int InternalCapacity;
	public float ReloadTime;
}
[Serializable]
public struct ChamberSettings
{
	public float ChamberTime;
}