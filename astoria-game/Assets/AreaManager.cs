using System;
using Player;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [SerializeField] private AreaData _snowArea;
    [SerializeField] private AreaData _desertArea;

    private Transform _playerTransform;

    [Serializable]
    private struct AreaData
    {
        public Texture2D Texture;
        public Transform Transform;
    }

    private void Start()
    {
        _playerTransform = GetComponentInParent<PlayerController>().transform;
    }

    private void Update()
    {
        Vector3 snowLocalPos = _snowArea.Transform.InverseTransformPoint(_playerTransform.position);
        Vector3 desertLocalPos = _desertArea.Transform.InverseTransformPoint(_playerTransform.position);

        if (IsPlayerInArea(_snowArea, snowLocalPos))
        {
            Debug.Log("Player is in the snow area.");
        }
        else if (IsPlayerInArea(_desertArea, desertLocalPos))
        {
            Debug.Log("Player is in the desert area.");
        }
        else
        {
            Debug.Log("Player is not in any defined area.");
        }
    }

    private bool IsPlayerInArea(AreaData area, Vector3 localPos)
    {
        // Use X and Z if your area lies on the X-Z plane
        Vector2 uv = new Vector2(localPos.x + 0.5f, localPos.z + 0.5f);

        if (uv.x < 0 || uv.x > 1 || uv.y < 0 || uv.y > 1)
            return false;

        int texX = Mathf.FloorToInt(uv.x * area.Texture.width);
        int texY = Mathf.FloorToInt(uv.y * area.Texture.height);

        Color pixel = area.Texture.GetPixel(texX, texY);

        return pixel.a > 0.5f;
    }
}
