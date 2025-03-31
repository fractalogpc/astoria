using UnityEngine;
using System.Collections.Generic;
using System;

public class CompassDisplay : MonoBehaviour
{
  [Tooltip("The transform of the camera to calculate icon positions.")]
  [SerializeField] private Transform cameraTransform;

  [Tooltip("The parent transform for direction icons.")]
  [SerializeField] private Transform directionsParent;

  [Tooltip("The parent transform for object icons.")]
  [SerializeField] private Transform objectsParent;

  [Header("Bar settings")]

  [Tooltip("The parent transform for object icons.")]
  [SerializeField] private Transform _barParent;

  [Tooltip("The sprite for the small fill bars.")]
  [SerializeField] private Sprite _barSprite;

  [Tooltip("The number of fill bars to display.")]
  [SerializeField] private int _barCount = 8;

  [Tooltip("The horizontal position scalar for cardinal directions.")]
  [SerializeField] private float directionHorizontalPositionScalar = 1.0f;

  [Tooltip("The horizontal position scalar for object icons.")]
  [SerializeField] private float iconsHorizontalPositionScalar = 1.0f;

  [Tooltip("The width of the compass UI in world units.")]
  [SerializeField] private float compassWidth = 100.0f;

  [Tooltip("Default scale for direction icons.")]
  [SerializeField] private float defaultDirectionIconScale = 0.5f;

  [Tooltip("Default alpha for direction icons.")]
  [SerializeField] private float defaultDirectionIconAlpha = 0.5f;

  [Tooltip("Default scale for object icons.")]
  [SerializeField] private float defaultObjectIconScale = 0.75f;

  [Tooltip("Default alpha for object icons.")]
  [SerializeField] private float defaultObjectIconAlpha = 1.0f;

  [Tooltip("Animation curve for icon scaling based on distance.")]
  [SerializeField] private AnimationCurve iconScaleCurve = AnimationCurve.Linear(0, 1, 1, 1);

  [Tooltip("Maximum distance for scaling using the icon scale curve.")]
  [SerializeField] private float maxScaleDistance = 50f;

  [Tooltip("Animation curve for icon alpha based on distance.")]
  [SerializeField] private AnimationCurve iconAlphaCurve = AnimationCurve.Linear(0, 1, 1, 1);
  [Tooltip("Animation curve for icon alpha based on distance when close.")]
  [SerializeField] private AnimationCurve nearAlphaCurve = AnimationCurve.Linear(0, 1, 1, 1);

  [Tooltip("Maximum distance for alpha adjustment using the icon alpha curve.")]
  [SerializeField] private float maxAlphaDistance = 50f;

  [Tooltip("Distance at which the near fade effect starts.")]
  [SerializeField] private float distanceToStartNearFade = 5f;

  [Tooltip("If true, key item icons do not scale with distance.")]
  [SerializeField] private bool disableScaleWithDistanceForKeyItems = true;

  [Tooltip("If true, key item icons do not fade with distance.")]
  [SerializeField] private bool disableAlphaWithDistanceForKeyItems = true;

  [Tooltip("If true, only apply near fade to key items.")]
  [SerializeField] private bool onlyNearFadeOnKeyItems = true;

  [Tooltip("Array of cardinal directions with associated angles and icons.")]
  [SerializeField] private CardinalDirection[] cardinalDirections;

  private readonly Dictionary<CompassIconObject, GameObject> objectIcons = new();
  private readonly Dictionary<CardinalDirection, GameObject> cardinalIcons = new();

  [Serializable]
  public struct CardinalDirection
  {
    public string name;
    public float angle;
    public Sprite sprite;
    public bool ignoreScale;
  }

  /// <summary>
  /// Initializes the compass display by setting up cardinal direction icons.
  /// </summary>
  private void Start()
  {
    InitializeCardinalDirections();
    InitializeFillBars();
  }

  private float stashedDirection = 0f;
  private Vector3 stashedPosition = Vector3.zero;
  /// <summary>
  /// Updates the compass display each frame.
  /// </summary>
  private void Update()
  {
    // Only update if the camera has rotated or player has moved
    if (stashedDirection != cameraTransform.eulerAngles.y)
    {
      stashedDirection = cameraTransform.eulerAngles.y;
    }
    else if (stashedPosition != cameraTransform.position)
    {
      stashedPosition = cameraTransform.position;
    }
    else
    {
      return;
    }
    UpdateCardinalDirections();
    UpdateCompassIcons();
  }

  private void InitializeCardinalDirections()
  {
    foreach (var direction in cardinalDirections)
    {
      GameObject icon = CreateCompassIcon(direction.sprite, $"{direction.name}_CompassIcon", directionsParent);
      cardinalIcons.Add(direction, icon);
    }
  }

  private void UpdateCardinalDirections()
  {
    foreach (var pair in cardinalIcons)
    {
      CardinalDirection direction = pair.Key;
      GameObject icon = pair.Value;

      Vector3 forward = Quaternion.Euler(0, direction.angle, 0) * Vector3.forward;
      forward.y = 0;
      forward.Normalize();

      UpdateIconPosition(forward, icon, directionHorizontalPositionScalar, false);
      if (!direction.ignoreScale)
      {
        UpdateIconScaleAndAlpha(icon, 0, false, true);
      }
    }
  }

  private void InitializeFillBars()
  {
    for (int i = 0; i < _barCount; i++)
    {
      GameObject bar = CreateCompassIcon(_barSprite, $"Bar_{i}", _barParent);

      // Resize bar
      bar.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 5);

      // Change the alpha of the bar
      bar.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.3f);

      // We can pretend the bar is a cardinal direction for the sake of positioning
      CardinalDirection barDirection = new CardinalDirection
      {
        name = $"Bar_{i}",
        angle = i * (360 / _barCount) + (360 / _barCount / 4),
        sprite = _barSprite,
        ignoreScale = true
      };
      cardinalIcons.Add(barDirection, bar);
    }
  }

  /// <summary>
  /// Adds an object to the compass display.
  /// </summary>
  /// <param name="compassObject">The object to add.</param>
  /// <returns>True if added successfully, false otherwise.</returns>
  public bool AddObjectToCompass(CompassIconObject compassObject)
  {
    if (objectIcons.ContainsKey(compassObject))
    {
      Debug.LogWarning($"CompassObject {compassObject.transform?.gameObject.name ?? "Unknown"} already exists.");
      return false;
    }

    GameObject icon = CreateCompassIcon(compassObject.icon, $"{compassObject.transform?.gameObject.name}_Icon", objectsParent);
    if (icon == null)
    {
      Debug.LogError($"Failed to create UI icon for {compassObject.transform?.gameObject.name ?? "Unknown"}.");
      return false;
    }

    objectIcons.Add(compassObject, icon);
    return true;
  }

  /// <summary>
  /// Removes an object from the compass display.
  /// </summary>
  /// <param name="compassObject">The object to remove.</param>
  /// <returns>True if removed successfully, false otherwise.</returns>
  public bool RemoveObjectFromCompass(CompassIconObject compassObject)
  {
    if (!objectIcons.TryGetValue(compassObject, out var icon))
    {
      Debug.LogWarning($"CompassObject {compassObject.transform?.gameObject.name ?? "Unknown"} does not exist.");
      return false;
    }

    Destroy(icon);
    objectIcons.Remove(compassObject);
    return true;
  }

  private void UpdateIconPosition(Vector3 direction, GameObject icon, float scalar, bool isKeyItem)
  {
    direction.y = 0;
    direction.Normalize();

    Vector3 forward = cameraTransform.forward;
    forward.y = 0;
    forward.Normalize();

    float angle = Vector3.SignedAngle(forward, direction, Vector3.up);
    float positionX = angle * scalar;

    if (isKeyItem)
    {
      float halfCompassWidth = compassWidth / 2;
      positionX = Mathf.Clamp(positionX, -halfCompassWidth, halfCompassWidth);
    }

    icon.GetComponent<RectTransform>().localPosition = new Vector3(positionX, 0, 0);
  }

  private void UpdateCompassIcons()
  {
    var keysToRemove = new List<CompassIconObject>();

    foreach (var pair in objectIcons)
    {
      CompassIconObject compassObject = pair.Key;
      GameObject icon = pair.Value;

      if (!ValidateCompassObject(compassObject))
      {
        keysToRemove.Add(compassObject);
        continue;
      }

      Vector3 direction = compassObject.transform.position - cameraTransform.position;
      float distance = direction.magnitude;
      direction.Normalize();

      UpdateIconPosition(direction, icon, iconsHorizontalPositionScalar, compassObject.keyItem);
      UpdateIconScaleAndAlpha(icon, distance, compassObject.keyItem, false);
    }

    foreach (var key in keysToRemove)
    {
      RemoveObjectFromCompass(key);
    }
  }

  private void UpdateIconScaleAndAlpha(GameObject icon, float distance, bool isKeyItem, bool isDirectionIcon)
  {
    UnityEngine.UI.Image image = icon.GetComponent<UnityEngine.UI.Image>();
    if (image == null) return;

    if (isDirectionIcon)
    {
      icon.transform.localScale = Vector3.one * defaultDirectionIconScale;
      Color color = image.color;
      color.a = defaultDirectionIconAlpha;
      image.color = color;
      return;
    }

    // Update scale
    if (!isKeyItem || !disableScaleWithDistanceForKeyItems)
    {
      float scale = iconScaleCurve.Evaluate(Mathf.Clamp01(distance / maxScaleDistance));
      icon.transform.localScale = Vector3.one * defaultObjectIconScale * scale;
    }
    else
    {
      icon.transform.localScale = Vector3.one * defaultObjectIconScale;
    }

    // Update alpha
    if (!isKeyItem || !disableAlphaWithDistanceForKeyItems)
    {
      float alpha = iconAlphaCurve.Evaluate(Mathf.Clamp01(distance / maxAlphaDistance));
      Color color = image.color;
      color.a = alpha;
      image.color = color;
    }
    else
    {
      Color color = image.color;
      color.a = defaultObjectIconAlpha;
      image.color = color;
    }
    if (!isKeyItem) return;
    // If the distance is less than the near fade distance, override alpha to use the nearAlphaCurve
    if (distance < distanceToStartNearFade)
    {
      float nearAlpha = nearAlphaCurve.Evaluate(1 - Mathf.Clamp01(distance / distanceToStartNearFade));
      Color color = image.color;
      color.a *= nearAlpha;
      image.color = color;
    }
  }

  private bool ValidateCompassObject(CompassIconObject compassObject)
  {
    return compassObject != null && compassObject.transform != null;
  }

  private GameObject CreateCompassIcon(Sprite sprite, string name, Transform parent)
  {
    GameObject icon = new GameObject(name);
    RectTransform rectTransform = icon.AddComponent<RectTransform>();
    rectTransform.SetParent(parent, false);

    var image = icon.AddComponent<UnityEngine.UI.Image>();
    image.sprite = sprite;

    return icon;
  }
}

public class CompassIconObject
{
  public Transform transform;
  public Sprite icon;
  public bool keyItem;

  public CompassIconObject(GameObject gameObject, Sprite icon, bool keyItem = false)
  {
    this.transform = gameObject?.transform;
    this.icon = icon;
    this.keyItem = keyItem;
  }
}