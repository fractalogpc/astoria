using Mirror;
using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.HighDefinition;

public class ReflectionProbeFollow : MonoBehaviour
{

  [SerializeField] private Transform _reflectionProbe;
  [SerializeField] private Transform _secondaryReflectionProbe;
  [SerializeField] private ReflectionProbe _reflectionProbeComponent;
  [SerializeField] private ReflectionProbe _secondaryReflectionProbeComponent;
  [SerializeField] private HDAdditionalReflectionData _reflectionProbeData;
  [SerializeField] private HDAdditionalReflectionData _secondaryReflectionProbeData;

  [SerializeField] private float _updateInterval = 0.5f;
  
  private Transform _localPlayer;

  private float _timeSinceLastUpdate = 0;
  private bool _toUpdatePrimary = true;

  private IEnumerator Start() {
    while (true) {
      if (_localPlayer == null) {
        if (NetworkClient.localPlayer == null) {
          yield return null;
          continue;
        }
        _localPlayer = NetworkClient.localPlayer.transform;
      } else {
        break;
      }
      yield return null;
    }
  }

  private void Update() {
    if (_localPlayer == null) return;

    if (_reflectionProbeComponent.IsFinishedRendering(0) && !_toUpdatePrimary) {
      // Blend the reflection probe towards weight 1

      _reflectionProbeData.weight = _timeSinceLastUpdate / _updateInterval;

      _timeSinceLastUpdate += Time.deltaTime;
    } else if (_secondaryReflectionProbeComponent.IsFinishedRendering(0) && _toUpdatePrimary) {
      // Blend the reflection probe towards weight 1

      _secondaryReflectionProbeData.weight = _timeSinceLastUpdate / _updateInterval;

      _timeSinceLastUpdate += Time.deltaTime;
    }
    
    if (_timeSinceLastUpdate >= _updateInterval) {
      _timeSinceLastUpdate = 0;

      if (_toUpdatePrimary) {
        _reflectionProbe.position = _localPlayer.position;

        _reflectionProbeComponent.RenderProbe();

        _reflectionProbeData.weight = 0;
      } else {
        _secondaryReflectionProbe.position = _localPlayer.position;

        _secondaryReflectionProbeComponent.RenderProbe();

        _secondaryReflectionProbeData.weight = 0;
      }

      _toUpdatePrimary = !_toUpdatePrimary;
    }

  }

}
