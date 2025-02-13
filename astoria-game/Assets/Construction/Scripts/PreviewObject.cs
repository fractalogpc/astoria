using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script attached to the preview object of either props or components. Handles logic for checking for collisions and setting the material.
/// </summary>
[ExecuteInEditMode]
public class PreviewObject : MonoBehaviour
{
  public ConstructionData Data;

  [System.Serializable]
  public class BoxData
  {
    public Vector3 position = Vector3.zero;
    public Vector3 size = Vector3.one;
    public Quaternion rotation = Quaternion.identity;
    public bool isExpanded = true; // Flag to toggle foldout
  }

  [HideInInspector] public float xRotation = 0;
  [HideInInspector] public List<BoxData> boxes = new List<BoxData>();

  [HideInInspector] public float heightOffset = 0f;


  public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
  {
    transform.SetPositionAndRotation(position, rotation * Quaternion.Euler(0, xRotation, 0));
  }

  // Check if the preview object is colliding with any other objects in its current position and rotation
  public bool IsColliding(LayerMask layer = default, Transform ignoreTransform = null)
  {
    foreach (BoxData box in boxes)
    {
      Vector3 worldPosition = transform.TransformPoint(box.position);

      // Adjust world extents based on transform's lossy scale to ensure matching physics size
      Vector3 scaledExtents = Vector3.Scale(box.size / 2f, transform.lossyScale);
      Quaternion worldRotation = box.rotation * transform.rotation;

      // Perform the overlap box check with the corrected extents and world rotation
      Collider[] hits = Physics.OverlapBox(worldPosition, scaledExtents, worldRotation, layer);

      foreach (Collider hit in hits)
      {
        if (hit.transform == transform || hit.transform == ignoreTransform) continue;
        return true; // Collision detected
      }
    }
    return false; // No collisions
  }

  // Check if the preview object is colliding with any other objects in a specific position and rotation
  public bool IsColliding(Vector3 position, Quaternion rotation, LayerMask layer = default, List<Transform> ignoreTransforms = null)
  {
    foreach (BoxData box in boxes)
    {
      // Compute the box's world position based on the custom position and rotation
      Vector3 worldPosition = position + rotation * box.position;

      // Adjust world extents based on transform's lossy scale to ensure matching physics size
      Vector3 scaledExtents = Vector3.Scale(box.size / 2f, transform.lossyScale);

      // Compute the box's world rotation based on the custom rotation
      Quaternion worldRotation = rotation * box.rotation;

      // Perform the overlap box check with the corrected extents and world rotation
      Collider[] hits = Physics.OverlapBox(worldPosition, scaledExtents, worldRotation, layer);

      foreach (Collider hit in hits)
      {
        if (hit.transform == transform || (ignoreTransforms != null && ignoreTransforms.Contains(hit.transform))) continue;
        return true; // Collision detected
      }
    }
    return false; // No collisions
  }


  public void SetMaterial(Material material)
  {
    if (material == null) return;

    Renderer[] renderers = GetComponentsInChildren<Renderer>();
    foreach (Renderer renderer in renderers)
    {
      renderer.material = material;
    }
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    foreach (BoxData box in boxes)
    {
      // Get the box position in world space
      Vector3 worldPosition = transform.TransformPoint(box.position);

      // Adjust world extents based on transform's lossy scale
      Vector3 scaledExtents = Vector3.Scale(box.size / 2f, transform.lossyScale);

      // Combine the rotation of the box with the object's transform rotation
      Quaternion combinedRotation = transform.rotation * box.rotation;

      // Set up the Gizmos matrix with combined position, rotation, and scale
      Gizmos.matrix = Matrix4x4.TRS(worldPosition, combinedRotation, Vector3.one);

      // Draw the wireframe cube to represent the box
      Gizmos.DrawWireCube(Vector3.zero, scaledExtents * 2f);
    }
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = new Color(0, 1, 1, 0.3f); // Semi-transparent cyan color

    // Calculate local position of the plane, keeping it aligned to the object's local Y-axis
    Vector3 localPlanePosition = new Vector3(0, heightOffset, 0);

    // Transform this local position to world space for accurate drawing
    Vector3 worldPlanePosition = transform.TransformPoint(localPlanePosition);

    // Draw the plane in world space with a small thickness
    Gizmos.DrawCube(worldPlanePosition, new Vector3(5, 0.01f, 5));
  }

}
