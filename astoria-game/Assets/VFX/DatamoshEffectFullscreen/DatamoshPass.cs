using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

class DatamoshPass : CustomPass
{
    /// <summary>
    /// Material used for the fullscreen pass, it's shader must have been created with.
    /// </summary>
    public Material fullscreenDatamoshMaterial;
    
    /// <summary>
    /// Called before the first execution of the pass occurs.
    /// Allow you to allocate custom buffers.
    /// </summary>
    /// <param name="renderContext">The render context</param>
    /// <param name="cmd">Current command buffer of the frame</param>
    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        
    }

    /// <summary>
    /// Execute the pass with the fullscreen setup
    /// </summary>
    /// <param name="ctx">The context of the custom pass. Contains command buffer, render context, buffer, etc.</param>
    protected override void Execute(CustomPassContext ctx) {
        if (fullscreenDatamoshMaterial == null || fullscreenDatamoshMaterial.passCount <= 0) return;
        
        if (Input.GetKey(KeyCode.B)) {
            fullscreenDatamoshMaterial.SetFloat("_DatamoshIntensity", 1);
        }
        else {
            fullscreenDatamoshMaterial.SetFloat("_DatamoshIntensity", 0);
        }
        
        CoreUtils.DrawFullScreen(ctx.cmd, fullscreenDatamoshMaterial);
    }
    protected override void Cleanup() {
        
    }
}