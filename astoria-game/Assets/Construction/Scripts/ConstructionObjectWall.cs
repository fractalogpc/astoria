using UnityEngine;

public class ConstructionObjectWall : MonoBehaviour
{
  public Vector3[] wallJoints; // Local space positions of each joint

  // Finds the closest joint in world space, ignores the Y-axis
  public Vector3 GetClosestJointWorldSpace(Vector3 point, out int jointIndex)
  {
    Vector2 point2D = new(point.x, point.z);
    Vector2 closest2D = new(transform.TransformPoint(wallJoints[0]).x, transform.TransformPoint(wallJoints[0]).z);
    float closestDistance = Vector2.Distance(point2D, closest2D);
    jointIndex = 0;

    for (int i = 1; i < wallJoints.Length; i++)
    {
      Vector3 jointWorldPosition = transform.TransformPoint(wallJoints[i]);
      Vector2 joint2D = new(jointWorldPosition.x, jointWorldPosition.z);
      float distance = Vector2.Distance(point2D, joint2D);
      if (distance < closestDistance)
      {
        closest2D = joint2D;
        closestDistance = distance;
        jointIndex = i;
      }
    }

    // Return the world-space joint position with Y value preserved
    return new Vector3(closest2D.x, transform.position.y, closest2D.y);
  }

    // Finds the closest joint in world space, ignores the Y-axis
  public Vector3 GetClosestJointWorldSpace(Vector3 point)
  {
    Vector2 point2D = new(point.x, point.z);
    Vector2 closest2D = new(transform.TransformPoint(wallJoints[0]).x, transform.TransformPoint(wallJoints[0]).z);
    float closestDistance = Vector2.Distance(point2D, closest2D);

    for (int i = 1; i < wallJoints.Length; i++)
    {
      Vector3 jointWorldPosition = transform.TransformPoint(wallJoints[i]);
      Vector2 joint2D = new(jointWorldPosition.x, jointWorldPosition.z);
      float distance = Vector2.Distance(point2D, joint2D);
      if (distance < closestDistance)
      {
        closest2D = joint2D;
        closestDistance = distance;
      }
    }

    // Return the world-space joint position with Y value preserved
    return new Vector3(closest2D.x, transform.position.y, closest2D.y);
  }

  public Vector3 GetJointWorldSpace(int index) {
    return transform.TransformPoint(wallJoints[index]);
  }
}
