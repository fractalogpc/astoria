using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Place Prop Objective Data", menuName = "Scriptable Objects/Objectives/Place Prop Objective", order = 0)]
public class PlacePropObjectiveData : ObjectiveData
{
    public ConstructionData PropConstructionData;

    public virtual ObjectiveInstance CreateInstance(ObjectiveSystemManager objectiveSystemManager) {
        return new PlacePropObjectiveInstance(this, objectiveSystemManager);
    }
}
