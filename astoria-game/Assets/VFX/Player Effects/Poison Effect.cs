using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

[Serializable, VolumeComponentMenu("Post-processing/Custom/Poison Effect")]
public sealed class PoisonEffect : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    [Tooltip("Controls the intensity of the effect.")]
    public ClampedIntParameter toggle = new ClampedIntParameter(0, 0, 1);
    public ClampedIntParameter loops = new ClampedIntParameter(5, 0, 8, true);
    public ClampedFloatParameter step = new ClampedFloatParameter(.015f, 0f, .02f, true);
    public ClampedFloatParameter stepAmout = new ClampedFloatParameter(.005f, 0f, .01f, true);
    public ClampedFloatParameter speed = new ClampedFloatParameter(1.03f, 0f, 2f, true);
    public ColorParameter tint = new ColorParameter(new Color(50f / 255f, 168f / 255f, 62f / 255f, 1), true);
    public ClampedFloatParameter mixAmount = new ClampedFloatParameter(.8f, 0f, 1f, true);
    public ClampedFloatParameter ChromAbbOffset = new ClampedFloatParameter(.015f, 0f, .02f, true);
    
    Material m_Material;

    public bool IsActive() => m_Material != null && toggle.value == 1;

    // Do not forget to add this post process in the Custom Post Process Orders list (Project Settings > Graphics > HDRP Global Settings).
    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    const string kShaderName = "Hidden/Shader/PoisonEffect";

    public override void Setup()
    {
        if (Shader.Find(kShaderName) != null)
            m_Material = new Material(Shader.Find(kShaderName));
        else
            Debug.LogError($"Unable to find shader '{kShaderName}'. Post Process Volume Poison Effect is unable to load. To fix this, please edit the 'kShaderName' constant in Poison Effect.cs or change the name of your custom post process shader.");
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (m_Material == null)
            return;

        m_Material.SetInteger("_Toggle", toggle.value);
        m_Material.SetInteger("_Loops", loops.value);
        m_Material.SetFloat("_Step", step.value);
        m_Material.SetFloat("_StepAmount", step.value);
        m_Material.SetFloat("_Speed", speed.value);
        m_Material.SetTexture("_MainTex", source);
        m_Material.SetColor("_Tint", tint.value);
        m_Material.SetFloat("_MixAmount", mixAmount.value);
        m_Material.SetFloat("_ChromAbbOffset", ChromAbbOffset.value);
        HDUtils.DrawFullScreen(cmd, m_Material, destination, shaderPassId: 0);
    }

    public override void Cleanup()
    {
        CoreUtils.Destroy(m_Material);
    }
}
