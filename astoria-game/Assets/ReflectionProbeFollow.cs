using Mirror;
using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;

public class ReflectionProbeFollow : MonoBehaviour
{

  [SerializeField] private Transform _reflectionProbe;
  [SerializeField] private Transform _secondaryReflectionProbe;
  [SerializeField] private ReflectionProbe _reflectionProbeComponent;
  [SerializeField] private ReflectionProbe _secondaryReflectionProbeComponent;
  [SerializeField] private HDAdditionalReflectionData _reflectionProbeData;
  [SerializeField] private HDAdditionalReflectionData _secondaryReflectionProbeData;

  [SerializeField] private float _updateInterval = 1f;
  
  private Transform _localPlayer;

  private float _timeSinceLastUpdate = 0;
  private bool _toUpdatePrimary = true;

  private int _waitForRenderFrames = 0;
  private int _renderFrameTime = 25;

  private IEnumerator Start() {
    _reflectionProbeComponent.timeSlicingMode = ReflectionProbeTimeSlicingMode.IndividualFaces;
    _secondaryReflectionProbeComponent.timeSlicingMode = ReflectionProbeTimeSlicingMode.IndividualFaces;

    while (true) {
      if (_localPlayer == null) {
        if (NetworkClient.localPlayer == null) {
          yield return null;
          continue;
        }
        _localPlayer = NetworkClient.localPlayer.transform;
        _timeSinceLastUpdate = _updateInterval;
      } else {
        break;
      }
      yield return null;
    }
  }

  private void Update() {
    if (_localPlayer == null) return;

    _waitForRenderFrames++;

    if (_waitForRenderFrames > _renderFrameTime && !_toUpdatePrimary) {
      // Blend the reflection probe towards weight 1

      _reflectionProbeData.weight = _timeSinceLastUpdate / _updateInterval;
      _secondaryReflectionProbeData.weight = 1 - _timeSinceLastUpdate / _updateInterval;

      _timeSinceLastUpdate += Time.deltaTime;
    } else if (_waitForRenderFrames > _renderFrameTime && _toUpdatePrimary) {
      // Blend the reflection probe towards weight 1

      _secondaryReflectionProbeData.weight = _timeSinceLastUpdate / _updateInterval;
      _reflectionProbeData.weight = 1 - _timeSinceLastUpdate / _updateInterval;

      _timeSinceLastUpdate += Time.deltaTime;
    }
    
    if (_timeSinceLastUpdate >= _updateInterval) {
      Debug.Log("Updating reflection probe");
      _timeSinceLastUpdate = 0;

      if (_toUpdatePrimary) {
        _reflectionProbe.position = _localPlayer.position;

        _reflectionProbeData.RequestRenderNextUpdate();

        _reflectionProbeData.weight = 0;
        _secondaryReflectionProbeData.weight = 1;

        _waitForRenderFrames = 0;
      } else {
        _secondaryReflectionProbe.position = _localPlayer.position;

        _secondaryReflectionProbeData.RequestRenderNextUpdate();

        _secondaryReflectionProbeData.weight = 0;
        _reflectionProbeData.weight = 1;

        _waitForRenderFrames = 0;
      }

      _toUpdatePrimary = !_toUpdatePrimary;
    }

  }

}
