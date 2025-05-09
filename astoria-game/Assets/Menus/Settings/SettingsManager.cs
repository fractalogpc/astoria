using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Events;

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
    public static int ShadowQuality { get; private set; } = 2; // 0 = Low, 1 = Medium, 2 = High
    public static int LightingQuality { get; private set; } = 1; // 0 = Low, 1 = High

    public static SettingsManager Instance { get; private set; }

    private int _framesSinceLastUpdate = 0;
    private const int _framesToWait = 5;
    private bool _queuedUpdate = false;

    public UnityEvent OnSettingsChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        ReloadSettings();
    }

    private void Start()
    {
        DynamicResolutionHandler.SetDynamicResScaler(() => RenderResolution, DynamicResScalePolicyType.ReturnsPercentage);
    }

    private void OnDestroy()
    {
        DynamicResolutionHandler.SetDynamicResScaler(() => 100.0f, DynamicResScalePolicyType.ReturnsPercentage);
    }

    private void Update()
    {
        _framesSinceLastUpdate++;

        if (_queuedUpdate)
        {
            if (_framesSinceLastUpdate >= _framesToWait)
            {
                ReloadSettings();
                _queuedUpdate = false;
                _framesSinceLastUpdate = 0;
            }
        }
    }
    
    public void SetRenderResolution(float value)
    {
        SetSettingFloat("RenderResolution", value);
    }

    public void SetShadowQuality(int value)
    {
        SetSettingInt("ShadowQuality", value);
    }

    public void SetLightingQuality(int value)
    {
        SetSettingInt("LightingQuality", value);
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
        // Check if we need to wait for the next frame to apply the settings
        if (_framesSinceLastUpdate < _framesToWait)
        {
            _queuedUpdate = true;
            return;
        }

        // Apply settings

        // Graphics
        float renderResolution = PlayerPrefs.GetFloat("RenderResolution", 100f);
        RenderResolution = renderResolution;
        Debug.Log("Render Resolution: " + renderResolution);

        int shadowQuality = PlayerPrefs.GetInt("ShadowQuality", 2);
        ShadowQuality = shadowQuality;
        Debug.Log("Shadow Quality: " + shadowQuality);

        int lightingQuality = PlayerPrefs.GetInt("LightingQuality", 1);
        LightingQuality = lightingQuality;
        Debug.Log("Lighting Quality: " + lightingQuality);

        OnSettingsChanged?.Invoke();
        Debug.Log("Settings reloaded.");
    }

}