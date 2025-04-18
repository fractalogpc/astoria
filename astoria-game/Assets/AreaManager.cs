using System;
using Player;
using UnityEngine;
using UnityEngine.Rendering;

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
        public Volume volume;
    }

    private void Start()
    {
        _playerTransform = GetComponentInParent<PlayerController>().transform;
    }

    private void Update()
    {
        UpdateAreaWeights();
    }

    private void UpdateAreaWeights()
    {
        Vector3 snowLocalPos = _snowArea.Transform.InverseTransformPoint(_playerTransform.position);
        Vector3 desertLocalPos = _desertArea.Transform.InverseTransformPoint(_playerTransform.position);

        float snowIntensity, desertIntensity;
        if (IsPlayerInArea(_snowArea, snowLocalPos, out snowIntensity))
        {
            _snowArea.volume.weight = snowIntensity;
        } else {
            _snowArea.volume.weight = 0f;
        }
        if (IsPlayerInArea(_desertArea, desertLocalPos, out desertIntensity))
        {
            _desertArea.volume.weight = desertIntensity;
        } else {
            _desertArea.volume.weight = 0f;
        }
    }

    private bool IsPlayerInArea(AreaData area, Vector3 localPos, out float intensity)
    {
        intensity = 0f;

        // Use X and Z if your area lies on the X-Z plane
        Vector2 uv = new Vector2(localPos.x + 0.5f, localPos.z + 0.5f);

        if (uv.x < 0 || uv.x > 1 || uv.y < 0 || uv.y > 1)
            return false;

        int texX = Mathf.FloorToInt(uv.x * area.Texture.width);
        int texY = Mathf.FloorToInt(uv.y * area.Texture.height);

        Color pixel = area.Texture.GetPixel(texX, texY);

        intensity = pixel.a;

        return pixel.a > 0.0f;
    }
}
