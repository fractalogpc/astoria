using System.Collections;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [SerializeField] private Transform _toFollow;

    private Transform _localPlayer;

    private IEnumerator Start() {
        while (true) {
                if (PlayerInstance.Instance == null) {
                    yield return null;
                    continue;
                }
                _localPlayer = PlayerInstance.Instance.transform;
            yield return null;
        }
    }

    void Update() {
        if (_localPlayer == null) return;

        _toFollow.position = _localPlayer.position;
    }

}
