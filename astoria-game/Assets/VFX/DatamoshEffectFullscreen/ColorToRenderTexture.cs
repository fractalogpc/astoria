using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

class ColorToRenderTexture : CustomPass
{
    public RenderTexture SceneColorTexture;
    
    // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
    // When empty this render pass will render to the active camera render target.
    // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
    // The render pipeline will ensure target setup and clearing happens in an performance manner.
    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd) {
        Camera mainCamera = Camera.main;
        if (mainCamera != null) {
            SceneColorTexture.Release();
            SceneColorTexture.width = mainCamera.pixelWidth;
            SceneColorTexture.height = mainCamera.pixelHeight;
            SceneColorTexture.Create();
        }
    }

    protected override void Execute(CustomPassContext ctx)
    {
        Graphics.Blit(ctx.cameraColorBuffer, SceneColorTexture);
    }

    protected override void Cleanup()
    {
        SceneColorTexture.Release();
    }
}