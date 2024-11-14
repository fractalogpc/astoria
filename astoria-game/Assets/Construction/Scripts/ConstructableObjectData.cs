using UnityEngine;

[CreateAssetMenu(fileName = "ConstructableObject", menuName = "Construction/ConstructableObject")]
public class ConstructableObjectData : ScriptableObject
{

  public enum ConstructableType
  {
    Default,
    Wall,
    Prop
  }

  public string Name;
  public ConstructableType Type;
  public GameObject TemporaryPrefab;
  public GameObject FinalPrefab;

  public float HeightOffset = 0;
  public Vector3 RotationOffset = Vector3.zero;
  public Vector3 PositionOffset = Vector3.zero;
}
