using UnityEngine;

public class SetSnowDisplacement : MonoBehaviour
{

  public int textureSize = 256;
  public float displacementScale = 1.0f;

  private Texture2D displacementTexture;
  [SerializeField] private Material snowMaterial;
  [SerializeField] private Transform playerTransform;

  public static SetSnowDisplacement Instance { get; private set; }

  private void Start() {
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
    snowMaterial.SetVector("_Center", new Vector4(playerTransform.position.x, playerTransform.position.z, 0.0f, 0.0f));
  }

  public void DisplacePoint(Vector3 point) {
    Vector2 uv = new Vector2(
      (point.x - playerTransform.position.x) / displacementScale + 0.5f,
      (point.z - playerTransform.position.z) / displacementScale + 0.5f
    );
    if (uv.x < 0.0f || uv.x > 1.0f || uv.y < 0.0f || uv.y > 1.0f) {
      return;
    }
    Debug.Log(uv);
    displacementTexture.SetPixel(
      (int)(uv.x * textureSize),
      (int)(uv.y * textureSize),
      new Color(0.5f, 0.0f, 0.0f, 0.0f)
    );

    displacementTexture.Apply();
  }

}