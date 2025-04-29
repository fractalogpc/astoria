using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Get Item Objective Data", menuName = "Scriptable Objects/Objectives/Get Item Objective")]
public class GetItemObjectiveData : LocationObjectiveData
{
    [Header("Get Item")]
    public ItemSetList RequiredItems;
    
    public override ObjectiveInstance CreateInstance(ObjectiveSystemManager objectiveSystemManager) {
        return new GetItemObjectiveInstance(this, objectiveSystemManager);
    }
}
