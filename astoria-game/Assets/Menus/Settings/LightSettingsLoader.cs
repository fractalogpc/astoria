using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightSettingsLoader : MonoBehaviour, ILoadSetting
{

    [SerializeField] private HDAdditionalLightData _lightData;

    private void Start()
    {
        ReloadSetting();

        SettingsManager.Instance.OnSettingsChanged.AddListener(ReloadSetting);
        // I like. Want to make this inherited somehow. But this interface thing doesn't work.
    }

    public void ReloadSetting()
    {
        Debug.Log("Reloading light settings" + SettingsManager.ShadowQuality);
        _lightData.shadowResolution.level = SettingsManager.ShadowQuality;
    }

}