using UnityEngine;
using UnityEngine.UI;

public class SettingsInteractor : MonoBehaviour
{

    [SerializeField] private Slider renderResolutionSlider;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        renderResolutionSlider.value = SettingsManager.RenderResolution;
        volumeSlider.value = SettingsManager.Volume;
    }
    
    public void SetRenderResolution(float value)
    {
        SettingsManager.Instance.SetRenderResolution(value);
    }

    public void SetShadowQuality(int value)
    {
        SettingsManager.Instance.SetShadowQuality(value);
    }

    public void SetLightingQuality(int value)
    {
        SettingsManager.Instance.SetLightingQuality(value);
    }

    public void SetVolume(float value)
    {
        SettingsManager.Instance.SetVolume(value);
    }

}
