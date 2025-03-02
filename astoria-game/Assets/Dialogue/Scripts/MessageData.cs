using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "MessageData", menuName = "Scriptable Objects/MessageData")]
public class MessageData : ScriptableObject
{
    public EventReference messageEvent;
    public List<string> messageText;
    public bool alwaysCaption;
    public float messageRange;
    public float messageDuration;
}
