using System;
using UnityEngine;

public abstract class ObjectiveData : ScriptableObject
{
    public string Title;
    public string Description;

    public abstract IObjective CreateInstance();
}
