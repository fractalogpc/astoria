using System;
using UnityEngine;

/// <summary>
/// An objective data with an associated location. Does not contain any automatic completion logic.
/// </summary>
[CreateAssetMenu(fileName = "Custom Location Objective Data", menuName = "Scriptable Objects/Objectives/Custom Location Objective")]
public class LocationObjectiveData : ObjectiveData
{
    [Tooltip("The icon that will be shown on the map for this objective.")]
    public Sprite MapMarkerIcon;
    
    public bool ShowOnMap = true;
    public bool ShowInWorld = true;

    public ObjectiveInstance CreateInstance(ObjectiveSystemManager objectiveSystemManager, Transform location) {
        return new LocationObjectiveInstance(this, objectiveSystemManager, location);
    }
}
