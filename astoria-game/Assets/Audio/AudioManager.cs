using FMOD.Studio;
using UnityEngine;
using FMODUnity;


/// <summary>
/// Use this script to interface with the FMOD sound system.
/// </summary>
public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance { get; private set; }

	/// <summary>
	/// Plays an FMOD event at the specified world position. 
	/// </summary>
	/// <param name="sound">The FMOD event to play.</param>
	/// <param name="worldPos">The world position to play at.</param>
	/// <returns>The EventInstance of the played sound. Useful for controlling the sound (stop, set parameters, etc).</returns>
	public EventInstance PlayOneShot(EventReference sound, Vector3 worldPos) {
		if (sound.IsNull) {
			Debug.LogError("Sound is null. Cannot play sound.");
			return default;
		}
		EventInstance instance = RuntimeManager.CreateInstance(sound);
		instance.set3DAttributes(RuntimeUtils.To3DAttributes(worldPos));
		instance.start();
		instance.release();
		return instance;
	}
	/// <summary>
	/// Plays an FMOD event, and attaches it to a game object. The sound's position will follow the game object.
	/// </summary>
	/// <param name="sound">The FMOD event to play.</param>
	/// <param name="gameObj">The gameObject to attach to.</param>
	/// <returns>The EventInstance of the played sound. Useful for controlling the sound (stop, set parameters, etc).</returns>
	public EventInstance PlayOneShotAttached(EventReference sound, GameObject gameObj) {
		if (sound.IsNull) {
			Debug.LogError("Sound is null. Cannot play sound.");
			return default;
		}
		EventInstance instance = RuntimeManager.CreateInstance(sound);
		RuntimeManager.AttachInstanceToGameObject(instance, gameObj.transform);
		instance.start();
		instance.release();
		return instance;
	}

	private void Awake() {
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}
}