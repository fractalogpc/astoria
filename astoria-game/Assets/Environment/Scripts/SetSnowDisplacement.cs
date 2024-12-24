using UnityEngine;

public class SetSnowDisplacement : MonoBehaviour
{

  private ComputeBuffer _snowDisplacementBuffer;
  private Vector4[] _snowDisplacement;
  [SerializeField] private Material _snowMaterial;
  [SerializeField] private int _positionsCount = 1000;

  private int _lastPositionUpdated = -1;

  public static SetSnowDisplacement Instance { get; private set; }

  public void DisplacePoint(Vector3 position) {
    _lastPositionUpdated = (_lastPositionUpdated + 1) % _positionsCount;
    _snowDisplacement[_lastPositionUpdated].x = position.x;
    _snowDisplacement[_lastPositionUpdated].y = position.y;
    _snowDisplacement[_lastPositionUpdated].z = position.z;
    _snowDisplacement[_lastPositionUpdated].w = 1.0f;
    _snowDisplacementBuffer.SetData(_snowDisplacement);
  }

  private void Start() {
    Instance = this;

    _snowDisplacementBuffer = new ComputeBuffer(_positionsCount, sizeof(float) * 4);
    _snowDisplacement = new Vector4[_positionsCount];
    for (int i = 0; i < _positionsCount; i++){
      _snowDisplacement[i] = new Vector4(0, 0, 0, 0);
    }
    _snowDisplacementBuffer.SetData(_snowDisplacement);
    _snowMaterial.SetInt("_PositionsCount", _positionsCount);
    _snowMaterial.SetBuffer("_SnowDisplacementBuffer", _snowDisplacementBuffer);
  }

  private void OnDisable() {
    _snowDisplacementBuffer.Release();
  }

}