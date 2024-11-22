using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConstructableData", menuName = "Scriptable Objects/ConstructableData")]
public class ConstructableObjectData : ItemData
{
  public enum ConstructableType
  {
    Default,
    Wall,
    Prop
  }

  [Header("Constructable Settings")]
  public List<ConstructionObjectCost> Cost = new();

  public ConstructableType Type;
  public GameObject TemporaryPrefab;
  public GameObject FinalPrefab;
  public GameObject FallingPrefab;

  public float HeightOffset = 0;
  public Vector3 RotationOffset = Vector3.zero;
  public Vector3 PositionOffset = Vector3.zero;
  public Vector3 HeldOffsetPosition = Vector3.zero;
  public Vector3 HeldOffsetRotation = Vector3.zero;

  public bool CarveNavmesh = true;
}

[System.Serializable]
public class ConstructionObjectCost {
  public ItemData Item;
  public int Amount;
  public ConstructionObjectCost(ItemData item, int amount) {
    Item = item;
    Amount = amount;
  }
}