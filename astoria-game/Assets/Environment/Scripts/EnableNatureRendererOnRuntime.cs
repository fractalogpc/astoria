using UnityEngine;
using VisualDesignCafe.Rendering.Nature;

public class EnableNatureRendererOnRuntime : MonoBehaviour
{

    private void Start() {
        // Find all children of this object that have a NatureRenderer component
        NatureRenderer[] natureRenderers = GetComponentsInChildren<NatureRenderer>();
        foreach (NatureRenderer natureRenderer in natureRenderers) {
            // Enable the renderer
            natureRenderer.enabled = true;
        }
    }

}
