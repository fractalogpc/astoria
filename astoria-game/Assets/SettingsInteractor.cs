using UnityEngine;
using UnityEngine.UI;

public class SettingsInteractor : MonoBehaviour
{

    [SerializeField] private Slider renderResolutionSlider;

    private void Start()
    {
        renderResolutionSlider.value = SettingsManager.RenderResolution;
    }
    
    public void SetRenderResolution(float value)
    {
        SettingsManager.Instance.SetRenderResolution(value);
    }

    public void SetShadowQuality(int value)
    {
        SettingsManager.Instance.SetShadowQuality(value);
    }

}
