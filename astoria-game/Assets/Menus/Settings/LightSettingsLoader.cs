using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightSettingsLoader : MonoBehaviour, ILoadSetting
{

    [SerializeField] private HDAdditionalLightData _lightData;

    public void ReloadSetting()
    {
        _lightData.shadowResolution.level = SettingsManager.ShadowQuality;
    }

}