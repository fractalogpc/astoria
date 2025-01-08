using System.Collections;
using UnityEngine;
using Mirror;

public class FollowPlayer : MonoBehaviour
{

    [SerializeField] private Transform _toFollow;

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

        _toFollow.position = _localPlayer.position;
    }

}
