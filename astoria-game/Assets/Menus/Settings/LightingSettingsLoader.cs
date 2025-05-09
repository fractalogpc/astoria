using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;

public class LightingSettingsLoader : MonoBehaviour, ILoadSetting
{

    [SerializeField] private Volume _volume;

    private void Start()
    {
        ReloadSetting();

        SettingsManager.Instance.OnSettingsChanged.AddListener(ReloadSetting);
        // I like. Want to make this inherited somehow. But this interface thing doesn't work.
    }

    public void ReloadSetting()
    {
        Debug.Log("Reloading lighting settings" + SettingsManager.LightingQuality);
        
        switch (SettingsManager.LightingQuality)
        {
            case 0:
                _volume.profile.TryGet<GlobalIllumination>(out var ssgi);
                ssgi.enable.Override(false);
                break;
            case 1:
                _volume.profile.TryGet<GlobalIllumination>(out var ssgi2);
                ssgi2.enable.Override(true);
                break;
            default:
                Debug.LogError("Invalid Lighting Quality setting: " + SettingsManager.LightingQuality);
                break;
        }
    }

}