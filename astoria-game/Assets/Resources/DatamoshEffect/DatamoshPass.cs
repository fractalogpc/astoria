using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

class DatamoshPass : CustomPass
{
    public RenderTexture DatamoshTexture;
    public RenderTexture MotionVectorTexture;
    public float DatamoshIntensity = 0.1f;
    public float CaptureInterval = 0.1f;
    public Material DatamoshMaterial;
    private float _timeSinceLastCapture = 0f;
    private RenderTexture _temp;
    protected override bool executeInSceneView => true;
    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        base.Setup(renderContext, cmd);
        // Create the temp RenderTexture
        _temp = new RenderTexture(DatamoshTexture.width, DatamoshTexture.height, 0);
    }
    protected override void Execute(CustomPassContext ctx)
    {
        // Blit motion vectors to the MotionVectorTexture every frame
        ctx.cmd.Blit(ctx.cameraMotionVectorsBuffer, MotionVectorTexture);
        // Capture a new "I-Frame" every CaptureInterval seconds
        _timeSinceLastCapture += Time.deltaTime;
        if(_timeSinceLastCapture >= CaptureInterval) {
            // Ensure RenderTexture is correctly sized
            DatamoshTexture.Release();
            DatamoshTexture.width = ctx.cameraColorBuffer.rt.width;
            DatamoshTexture.height = ctx.cameraColorBuffer.rt.height;
            DatamoshTexture.Create();
            Debug.Log("Capturing scene color.");
            _timeSinceLastCapture = 0f;
            // scale is the ratio of the render texture to the screen
            var scale = RTHandles.rtHandleProperties.rtHandleScale;
            // Blit renders the cameraColorBuffer to the DatamoshTexture
            ctx.cmd.Blit(ctx.cameraColorBuffer, DatamoshTexture, new Vector2(scale.x, scale.y), Vector2.zero, 0, 0);
        }
        if(DatamoshMaterial == null) return;
        // Ensure the temp RenderTexture is correctly sized
        _temp.Release();
        _temp.width = ctx.cameraColorBuffer.rt.width;
        _temp.height = ctx.cameraColorBuffer.rt.height;
        _temp.Create();
        // Run the shader on the DatamoshTexture
        ctx.cmd.Blit(DatamoshTexture.colorBuffer, _temp, DatamoshMaterial);
        ctx.cmd.Blit(_temp, DatamoshTexture);
    }
    protected override void Cleanup()
    {
        base.Cleanup();
        _temp.Release();
        DatamoshMaterial = null;
    }
}