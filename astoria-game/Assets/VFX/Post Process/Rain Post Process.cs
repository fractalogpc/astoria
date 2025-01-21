using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

[Serializable, VolumeComponentMenu("Post-processing/Custom/Rain Post Process")]
public sealed class RainPostProcess : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    [Tooltip("Controls the intensity of the effect.")]
    public ClampedIntParameter intensity = new ClampedIntParameter(0, 0, 20);
    private Vector4[] _RainDrops = new Vector4[20];

    Material m_Material;

    public bool IsActive() => m_Material != null && intensity.value > 0f;

    // Do not forget to add this post process in the Custom Post Process Orders list (Project Settings > Graphics > HDRP Global Settings).
    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    const string kShaderName = "Hidden/Shader/RainPostProcess";

    public override void Setup()
    {
        if (Shader.Find(kShaderName) != null)
            m_Material = new Material(Shader.Find(kShaderName));
        else
            Debug.LogError($"Unable to find shader '{kShaderName}'. Post Process Volume Rain Post Process is unable to load. To fix this, please edit the 'kShaderName' constant in Rain Post Process.cs or change the name of your custom post process shader.");
    }

    void Update() {
        Debug.Log("Test");
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (m_Material == null)
            return;

        for (int i = 0; i < 20; i++) {
            if (i < (int)intensity) {
                if (_RainDrops[i].z <= 0) {
                    _RainDrops[i] = new Vector3(UnityEngine.Random.Range(-20, 20), UnityEngine.Random.Range(-20, 20), UnityEngine.Random.Range(.2f, 5));
                } else {
                    _RainDrops[i].z -= Time.deltaTime;
                }
            } else {
                _RainDrops[i].z = 0;
            }
            
        }

        m_Material.SetFloat("_Intensity", intensity.value);
        m_Material.SetVectorArray("_RainDrops", _RainDrops);
        m_Material.SetTexture("_MainTex", source);
        HDUtils.DrawFullScreen(cmd, m_Material, destination, shaderPassId: 0);


    }

    public override void Cleanup()
    {
        CoreUtils.Destroy(m_Material);
    }
}
