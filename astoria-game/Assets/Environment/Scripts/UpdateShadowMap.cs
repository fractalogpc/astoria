using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;

public class UpdateShadowMap : MonoBehaviour
{

    [SerializeField] private HDAdditionalLightData _lightData;
    [SerializeField] private int _framesPerUpdate = 1;

    private int _frameCount = 0;

    private void Update() {
        _frameCount++;
        if (_frameCount >= _framesPerUpdate) {
            _frameCount = 0;
            _lightData.RequestShadowMapRendering();
        }
    }
}
