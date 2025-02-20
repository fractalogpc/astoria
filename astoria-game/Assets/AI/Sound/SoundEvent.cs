using SteamAudio;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class SoundEvent
{
    public Vector3 position;
    public float intensity;
    public string soundName;
    public GameObject source;

    public SoundEvent(Vector3 _position, float _intensity, string _soundName, GameObject _source)
    {
        position = _position;
        intensity = _intensity;
        soundName = _soundName;
        source = _source;
    }
}