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

  public static SetSnowDisplacement Instance { get; private set; }

  private IEnumerator Start() {
    Instance = this;
    displacementTexture = new Texture2D(textureSize, textureSize, TextureFormat.RFloat, false);
    Color[] colors = new Color[textureSize * textureSize];
    for (int i = 0; i < colors.Length; i++) {
      colors[i] = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }
    displacementTexture.SetPixels(colors);
    displacementTexture.Apply();
    snowMaterial.SetTexture("_SnowDisplacementTexture", displacementTexture);
    snowMaterial.SetFloat("_SnowDisplacementTextureSize", displacementScale);
    center = new Vector2(0, 0);
    snowMaterial.SetVector("_Center", new Vector4(center.x, center.y, 0.0f, 0.0f));
    while (_localPlayer == null) {
      yield return null;
      _localPlayer = NetworkClient.localPlayer?.transform;
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

    if (xDistance > displacementScale / textureSize) {
      center.x += displacementScale / textureSize;
      // Shift all pixels in the texture to the left
      Color[] colors = displacementTexture.GetPixels();
      
      for (int x = 0; x < textureSize; x++) {
        for (int y = 0; y < textureSize; y++) {
          if (x == textureSize - 1) {
            colors[x + y * textureSize] = new Color(0.0f, 0.0f, 0.0f, 0.0f);
          } else {
            colors[x + y * textureSize] = colors[x + 1 + y * textureSize];
          }
        }
      }

      displacementTexture.SetPixels(colors);
      updateTexture = true;
    } else if (xDistance < -displacementScale / textureSize) {
      center.x -= displacementScale / textureSize;
      // Shift all pixels in the texture to the right
      Color[] colors = displacementTexture.GetPixels();

      for (int x = textureSize - 1; x >= 0; x--) {
        for (int y = 0; y < textureSize; y++) {
          if (x == 0) {
            colors[x + y * textureSize] = new Color(0.0f, 0.0f, 0.0f, 0.0f);
          } else {
            colors[x + y * textureSize] = colors[x - 1 + y * textureSize];
          }
        }
      }
      
      displacementTexture.SetPixels(colors);
      updateTexture = true;
    }

    if (zDistance > displacementScale / textureSize) {
      center.y += displacementScale / textureSize;
      // Shift all pixels in the texture down
      Color[] colors = displacementTexture.GetPixels();

      for (int x = 0; x < textureSize; x++) {
        for (int y = 0; y < textureSize; y++) {
          if (y == textureSize - 1) {
            colors[x + y * textureSize] = new Color(0.0f, 0.0f, 0.0f, 0.0f);
          } else {
            colors[x + y * textureSize] = colors[x + (y + 1) * textureSize];
          }
        }
      }
      
      displacementTexture.SetPixels(colors);
      updateTexture = true;
    } else if (zDistance < -displacementScale / textureSize) {
      center.y -= displacementScale / textureSize;
      // Shift all pixels in the texture up
      Color[] colors = displacementTexture.GetPixels();
      
      for (int x = 0; x < textureSize; x++) {
        for (int y = textureSize - 1; y >= 0; y--) {
          if (y == 0) {
            colors[x + y * textureSize] = new Color(0.0f, 0.0f, 0.0f, 0.0f);
          } else {
            colors[x + y * textureSize] = colors[x + (y - 1) * textureSize];
          }
        }
      }

      displacementTexture.SetPixels(colors);
      updateTexture = true;
    }

    if (updateTexture) {
      displacementTexture.Apply();
      _snowPlane.position = new Vector3(center.x, 0.5f, center.y);
      snowMaterial.SetVector("_Center", new Vector4(center.x, center.y, 0.0f, 0.0f));
    }
  }

  public void DisplacePoint(Vector3 point) {
    Vector2 uv = new Vector2(
      (point.x - _localPlayer.position.x) / displacementScale + 0.5f,
      (point.z - _localPlayer.position.z) / displacementScale + 0.5f
    );
    if (uv.x < 0.0f || uv.x > 1.0f || uv.y < 0.0f || uv.y > 1.0f) {
      return;
    }
    Debug.Log(uv);
    displacementTexture.SetPixel(
      (int)(uv.x * textureSize),
      (int)(uv.y * textureSize),
      new Color(_displacementMagnitude, 0.0f, 0.0f, 0.0f)
    );

    displacementTexture.Apply();
  }

}