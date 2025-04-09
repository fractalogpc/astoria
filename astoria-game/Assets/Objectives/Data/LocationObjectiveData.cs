using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Location Objective Data", menuName = "Scriptable Objects/Objectives/Location Objective")]
public class LocationObjectiveData : ObjectiveData
{
    [Tooltip("The location the player must reach to complete the objective.")]
    public Vector3 Location;
    [Tooltip("The distance from the location the player must be within to complete the objective.")]
    public float CompletionDistance = 1f;

    public override ObjectiveInstance CreateInstance(ObjectiveSystemManager objectiveSystemManager) {
        return new LocationObjectiveInstance(this, objectiveSystemManager);
    }
}
