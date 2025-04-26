using UnityEngine;
using UnityEngine.Rendering;

public class SettingsManager : MonoBehaviour
{

    /* Settings Documentation
    Graphics:
        Render Resolution: 50-100%, default 100%
        Anti-Aliasing: TAA, FXAA, SMAA, Off, default TAA
        Shadow Quality: Low, Medium, High, default Medium
        Lighting Quality: Medium, High, default High

    Controls:
        Sensitivity: 0.1 - 10.0, default 1.0
    */

    public static float RenderResolution { get; private set; } = 100f;

    private void Start()
    {
        DynamicResolutionHandler.SetDynamicResScaler(() => 5.0f, DynamicResScalePolicyType.ReturnsPercentage);
    }

    public void SetRenderResolution(float value)
    {
        SetSettingFloat("RenderResolution", value);
    }

    private void SetSettingFloat(string settingName, float value)
    {
        PlayerPrefs.SetFloat(settingName, value);
        PlayerPrefs.Save();

        ReloadSettings();
    }

    private void SetSettingInt(string settingName, int value)
    {
        PlayerPrefs.SetInt(settingName, value);
        PlayerPrefs.Save();

        ReloadSettings();
    }

    private void ReloadSettings()
    {
        // Apply settings

        // Graphics
        float renderResolution = PlayerPrefs.GetFloat("RenderResolution", 100f);
        Debug.Log("Render Resolution: " + renderResolution);

        Debug.Log("Settings reloaded.");
    }

}