using UnityEngine;
using FMODUnity;

public class VolumeSettingsLoader : MonoBehaviour, ILoadSetting
{

    private void Start()
    {
        ReloadSetting();

        SettingsManager.Instance.OnSettingsChanged.AddListener(ReloadSetting);
        // I like. Want to make this inherited somehow. But this interface thing doesn't work.
    }

    public void ReloadSetting()
    {
        Debug.Log("Reloading volume settings" + SettingsManager.Volume);

        FMOD.Studio.Bus masterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
        masterBus.setVolume(SettingsManager.Volume);
    }

}