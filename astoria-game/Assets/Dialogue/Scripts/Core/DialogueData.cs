using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "MessageData", menuName = "Scriptable Objects/MessageData")]
public class DialogueData : ScriptableObject
{
		public string DialogueName;
    public List<string> MessageText;
    public EventReference AudioEvent;
    public bool AlwaysCaption;
    public float RecieveRange;
    public LayerMask RecieveLayerMask;
    public float Duration;
    
    public virtual Dialogue CreateInstance()
    {
        return new Dialogue(this);
    }
}
