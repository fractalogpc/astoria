using UnityEngine;
/// <summary>
/// This class is designed to be inherited by classes that handle player vitals.
/// This way, UI scripts can subscribe to the OnVitalChange event to update the UI.
/// The polymorphism allows for different types of vitals to be handled by the same UI script.
/// </summary>
public abstract class PlayerVitalHandler : MonoBehaviour
{
	/// <summary>
	/// Scripts use this to grab the current value of the vital.
	/// </summary>
	/// <returns>The current value of the vital.</returns>
	public abstract float GetCurrentValue();
	/// <summary>
	/// Scripts use this to grab the maximum value of the vital.
	/// </summary>
	/// <returns>The maximum value of the vital.</returns>
	public abstract float GetMaxValue();

	/// <summary>
	/// When implementing this interface, make sure to call this whenever the vital changes.
	/// UI scripts can subscribe to this event to update the UI.
	/// </summary>
	public event VitalChangeCallback OnVitalChange;
	public delegate void VitalChangeCallback(float intial, float current, float max);
	
	protected void NotifyVitalChange(float initial, float current, float max) {
		OnVitalChange?.Invoke(initial, current, max);
	}
}