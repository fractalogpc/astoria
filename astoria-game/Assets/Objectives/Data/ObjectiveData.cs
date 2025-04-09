using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Objective Data", menuName = "Scriptable Objects/Objectives/Base Objective", order = 0)]
public class ObjectiveData : ScriptableObject
{
    public string Title;
    public string Description;

    public virtual ObjectiveInstance CreateInstance(ObjectiveSystemManager objectiveSystemManager) {
        return new ObjectiveInstance(this, objectiveSystemManager);
    }
}
