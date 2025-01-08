using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Structure", menuName = "Scriptable Objects/Construction/Structure")]
public class ConstructionStructureData : ConstructionData
{
    [Header("Cost")]
    public List<ItemSet> Cost = new(); // TODO: Change to Matthew's system
}
