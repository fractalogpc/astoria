using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConstructableData", menuName = "Scriptable Objects/Deprecated/ConstructableData")]
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

  // Copied & adapted from CraftingStationNetworked.cs
  public bool MeetsCost(List<ItemInstance> playerItems) {
    // For every ingredient set in the recipe
    foreach (var costSet in Cost) {
      int ingredientCountLeft = costSet.Amount;
			
      // Scan through player items to find the ingredients
      foreach (ItemInstance item in playerItems) {
        if (item.ItemData != costSet.Item) continue;
        ingredientCountLeft--;
        if (ingredientCountLeft == 0) break;
      }

      if (ingredientCountLeft > 0) {
        // print($"Could not craft {recipeCount} of {recipe._resultSetList[0]._item.ItemName} because of not enough {ingredientSet._item.ItemName}");
        return false;
      }
    }
    // print($"Can craft {recipeCount} of {recipe._resultSetList[0]._item.ItemName}");
    return true;
  }
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