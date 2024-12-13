using Mirror;
using UnityEngine;
using System.Collections;

public class ReflectionProbeFollow : MonoBehaviour
{

  [SerializeField] private Transform _reflectionProbe;
  
  private Transform _localPlayer;

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

  void Update() {
    if (_localPlayer == null) return;
    _reflectionProbe.position = _localPlayer.position;
  }

}
