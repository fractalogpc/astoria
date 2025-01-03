using UnityEngine;
using System.Collections.Generic;
using System;

public class CompassDisplay : MonoBehaviour
{
    public Transform cameraTransform;

    public Transform parent;

    public float cardinalDirectionsScalar = 1.0f; // Scale for cardinal directions
    public float iconsScalar = 1.0f; // Scale for object icons
    public float compassWidth = 100.0f; // Width of the compass UI

    public AnimationCurve iconScaleCurve = AnimationCurve.Linear(0, 1, 1, 1);
    public float maxScaleDistance = 50f; // Distance at which the icon is at iconScaleCurve.evaluate(1) scale
    public AnimationCurve iconAlphaCurve = AnimationCurve.Linear(0, 1, 1, 1);
    public float maxAlphaDistance = 50f; // Distance at which the icon is at iconAlphaCurve.evaluate(1) alpha

    public bool disableScaleWithDistanceForKeyItems = true;
    public bool disableAlphaWithDistanceForKeyItems = true;

    [Serializable]
    public struct CardinalDirection
    {
        public string name;
        public float angle;
        public Sprite sprite;
    }

    public CardinalDirection[] cardinalDirections;

    private Dictionary<CompassIconObject, GameObject> objectIcons = new();
    private Dictionary<CardinalDirection, GameObject> cardinalIcons = new();

    private void Start()
    {
        InitializeCardinalDirections();
    }

    private void Update()
    {
        UpdateCardinalDirections();
        UpdateCompassIcons();
    }

    private void InitializeCardinalDirections()
    {
        foreach (var direction in cardinalDirections)
        {
            GameObject icon = CreateCompassIcon(direction.sprite, $"{direction.name}_CompassIcon");
            cardinalIcons.Add(direction, icon);
        }
    }

    private void UpdateCardinalDirections()
    {
        foreach (var pair in cardinalIcons)
        {
            CardinalDirection direction = pair.Key;
            GameObject icon = pair.Value;

            // Calculate the world direction based on the angle relative to the camera's forward direction
            Vector3 forward = Quaternion.Euler(0, direction.angle, 0) * Vector3.forward;

            // Project the direction onto the horizontal plane
            forward.y = 0;
            forward.Normalize();

            // Use this direction to calculate the position on the compass UI
            UpdateIconPosition(forward, icon, cardinalDirectionsScalar, false);
        }
    }

    public bool AddObjectToCompass(CompassIconObject compassObject)
    {
        if (objectIcons.ContainsKey(compassObject))
        {
            Debug.LogWarning($"CompassObject {compassObject.transform?.gameObject.name ?? "Unknown"} already exists.");
            return false;
        }

        GameObject icon = CreateCompassIcon(compassObject.icon, $"{compassObject.transform?.gameObject.name}_Icon");
        if (icon == null)
        {
            Debug.LogError($"Failed to create UI icon for {compassObject.transform?.gameObject.name ?? "Unknown"}.");
            return false;
        }

        objectIcons.Add(compassObject, icon);
        return true;
    }

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
        direction.y = 0; // Ignore vertical differences
        direction.Normalize();

        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();

        float angle = Vector3.SignedAngle(forward, direction, Vector3.up);

        // Determine the local X position for the icon
        float positionX = angle * scalar;

        // If the object is a key item, clamp it to the compass edges
        if (isKeyItem)
        {
            float halfCompassWidth = compassWidth / 2;
            positionX = Mathf.Clamp(positionX, -halfCompassWidth, halfCompassWidth);
        }

        // Set the icon's position based on the calculated or clamped value
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

            // Normalize the direction for angle calculation
            direction.Normalize();

            // Update position, scaling, and alpha
            UpdateIconPosition(direction, icon, iconsScalar, compassObject.keyItem);
            UpdateIconScaleAndAlpha(icon, distance, compassObject.keyItem);
        }

        foreach (var key in keysToRemove)
        {
            RemoveObjectFromCompass(key);
        }
    }

    private void UpdateIconScaleAndAlpha(GameObject icon, float distance, bool isKeyItem)
    {
        UnityEngine.UI.Image image = icon.GetComponent<UnityEngine.UI.Image>();
        if (image == null) return;

        // Scale logic
        if (!isKeyItem || !disableScaleWithDistanceForKeyItems)
        {
            float scale = iconScaleCurve.Evaluate(Mathf.Clamp01(distance / maxScaleDistance));
            icon.transform.localScale = Vector3.one * scale;
        }
        else
        {
            icon.transform.localScale = Vector3.one; // Default scale for key items if scaling is disabled
        }

        // Alpha logic
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
            color.a = 1.0f; // Default alpha for key items if alpha adjustment is disabled
            image.color = color;
        }
    }


    private bool ValidateCompassObject(CompassIconObject compassObject)
    {
        return compassObject != null && compassObject.transform != null;
    }

    private GameObject CreateCompassIcon(Sprite sprite, string name)
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
