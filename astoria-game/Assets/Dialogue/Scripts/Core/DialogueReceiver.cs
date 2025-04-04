using UnityEngine;

public interface DialogueReceiver
{
    public delegate void DialogueRecieved(Dialogue dialogue);
    public event DialogueRecieved OnDialogueRecieved;
    public void RecieveDialogue(Dialogue dialogue);
}
