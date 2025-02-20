using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private List<IListener> listeners = new List<IListener>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterListener(IListener listener)
    {
        if (!listeners.Contains(listener)) {
            listeners.Add(listener);
        }
    }

    public void UnregisterListener(IListener listener)
    {
        listeners.Remove(listener);
    }

    public void EmitSound(SoundEvent soundEvent)
    {
        Debug.Log("Sound emitted: " + soundEvent.soundName);
        foreach (var listener in listeners) {
            listener.OnSoundHeard(soundEvent);
        }
    }
}