using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Objective Data", menuName = "Scriptable Objects/Objectives/Base Objective", order = 0)]
public class ObjectiveData : ScriptableObject
{
    [Tooltip("The title of the objective, shown across the UI.")]
    public string Title;
    [Tooltip("The description of the objective. Use this to tell the player what/why they are doing this objective.")]
    [TextArea(4, 12)]
    public string Description;
    [Tooltip("The objective that will start when this one is completed. Leave empty if this is the last objective.")]
    public ObjectiveData NextObjectiveOptional;

    public virtual ObjectiveInstance CreateInstance(ObjectiveSystemManager objectiveSystemManager) {
        return new ObjectiveInstance(this, objectiveSystemManager);
    }
}
