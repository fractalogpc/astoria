using UnityEngine;
using Mirror;
using System.Collections;

public class SetSnowDisplacement : MonoBehaviour
{

  public int textureSize = 256;
  [SerializeField] private int _texelsPerVertex = 4;
  public float displacementScale = 1.0f;

  private Texture2D displacementTexture;
  [SerializeField] private Material snowMaterial;
  private Transform _localPlayer;
  [SerializeField] private Transform _snowPlane;
  [SerializeField] private float _displacementMagnitude;
  private Vector2 center;
  [SerializeField] private float _displacementRadius = 1.0f;

  public static SetSnowDisplacement Instance { get; private set; }

  private IEnumerator Start() {
    _snowPlane.GetComponent<MeshFilter>().mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 1000.0f);
    Instance = this;
    displacementTexture = new Texture2D(textureSize, textureSize, TextureFormat.RFloat, false);
    Color32[] colors = new Color32[textureSize * textureSize];
    for (int i = 0; i < colors.Length; i++) {
      colors[i] = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }
    displacementTexture.SetPixels32(colors);
    displacementTexture.Apply();
    snowMaterial.SetTexture("_SnowDisplacementTexture", displacementTexture);
    snowMaterial.SetFloat("_SnowDisplacementTextureSize", displacementScale);
    center = new Vector2(_snowPlane.position.x, _snowPlane.position.z);
    snowMaterial.SetVector("_Center", new Vector4(center.x, center.y, 0.0f, 0.0f));
    while (_localPlayer == null) {
      yield return null;
      _localPlayer = PlayerInstance.Instance?.transform;
    }
  }

  private void Update() {
    if (_localPlayer == null) {
      return;
    }

    // Update the center of the displacement texture
    float xDistance = _localPlayer.position.x - center.x;
    float zDistance = _localPlayer.position.z - center.y;

    bool updateTexture = false;

    int texelsToUpdate = _texelsPerVertex;
    float metersPerTexel = displacementScale / textureSize;

    if (xDistance > metersPerTexel * texelsToUpdate) {
      center.x += metersPerTexel * texelsToUpdate;
      // Shift all pixels in the texture to the left
      Color32[] colors = displacementTexture.GetPixels32();

      for (int x = 0; x < textureSize; x++) {
        for (int y = 0; y < textureSize; y++) {
          if (x >= textureSize - texelsToUpdate) {
            colors[x + y * textureSize] = new Color(0.0f, 0.0f, 0.0f, 0.0f);
          } else {
            colors[x + y * textureSize] = colors[x + texelsToUpdate + y * textureSize];
          }
        }
      }

      displacementTexture.SetPixels32(colors);
      updateTexture = true;
    } else if (xDistance < -metersPerTexel * texelsToUpdate) {
      center.x -= metersPerTexel * texelsToUpdate;
      // Shift all pixels in the texture to the right
      Color32[] colors = displacementTexture.GetPixels32();

      for (int x = textureSize - 1; x >= 0; x--) {
        for (int y = 0; y < textureSize; y++) {
          if (x < texelsToUpdate) {
            colors[x + y * textureSize] = new Color(0.0f, 0.0f, 0.0f, 0.0f);
          } else {
            colors[x + y * textureSize] = colors[x - texelsToUpdate + y * textureSize];
          }
        }
      }
      
      displacementTexture.SetPixels32(colors);
      updateTexture = true;
    }

    if (zDistance > metersPerTexel * texelsToUpdate) {
      center.y += metersPerTexel * texelsToUpdate;
      // Shift all pixels in the texture down
      Color32[] colors = displacementTexture.GetPixels32();

      for (int x = 0; x < textureSize; x++) {
        for (int y = 0; y < textureSize; y++) {
          if (y >= textureSize - texelsToUpdate) {
            colors[x + y * textureSize] = new Color(0.0f, 0.0f, 0.0f, 0.0f);
          } else {
            colors[x + y * textureSize] = colors[x + (y + texelsToUpdate) * textureSize];
          }
        }
      }
      
      displacementTexture.SetPixels32(colors);
      updateTexture = true;
    } else if (zDistance < -metersPerTexel * texelsToUpdate) {
      center.y -= metersPerTexel * texelsToUpdate;
      // Shift all pixels in the texture up
      Color32[] colors = displacementTexture.GetPixels32();
      
      for (int x = 0; x < textureSize; x++) {
        for (int y = textureSize - 1; y >= 0; y--) {
          if (y < texelsToUpdate) {
            colors[x + y * textureSize] = new Color(0.0f, 0.0f, 0.0f, 0.0f);
          } else {
            colors[x + y * textureSize] = colors[x + (y - texelsToUpdate) * textureSize];
          }
        }
      }

      displacementTexture.SetPixels32(colors);
      updateTexture = true;
    }

    if (updateTexture) {
      displacementTexture.Apply();
      _snowPlane.position = new Vector3(center.x, _snowPlane.position.y, center.y);
      snowMaterial.SetVector("_Center", new Vector4(center.x, center.y, 0.0f, 0.0f));
    }
  }
  public void DisplacePoint(Vector3 point) {
    // Find snow position at the point
    // Raycast down to terrain
    // RaycastHit hit;
    // if (!Physics.Raycast(point + Vector3.up * 100.0f, Vector3.down, out hit, 200.0f, LayerMask.GetMask("Ground"))) {
    //   return;
    // }
    if (point.y > _snowPlane.position.y/* + hit.point.y*/) {
      return;
    }
    Vector2 uv = new Vector2(
      (point.x - center.x) / displacementScale + 0.5f,
      (point.z - center.y) / displacementScale + 0.5f
    );
    if (uv.x < 0.0f || uv.x > 1.0f || uv.y < 0.0f || uv.y > 1.0f) {
      return;
    }

    int pixelRadius = Mathf.CeilToInt(_displacementRadius * textureSize / displacementScale);
    int centerX = (int)(uv.x * textureSize);
    int centerY = (int)(uv.y * textureSize);

    for (int x = -pixelRadius; x <= pixelRadius; x++) {
      for (int y = -pixelRadius; y <= pixelRadius; y++) {
        int pixelX = centerX + x;
        int pixelY = centerY + y;
        if (pixelX >= 0 && pixelX < textureSize && pixelY >= 0 && pixelY < textureSize) {
          float distance = Mathf.Sqrt(x * x + y * y) / pixelRadius;
          if (distance <= 1.0f) {
            float displacement = _displacementMagnitude * (1.0f - distance);
            Color currentColor = displacementTexture.GetPixel(pixelX, pixelY);
            displacementTexture.SetPixel(pixelX, pixelY, new Color(Mathf.Max(currentColor.r, displacement), 0.0f, 0.0f, 0.0f));
          }
        }
      }
    }

    displacementTexture.Apply();
  }

}