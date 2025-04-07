using UnityEngine;

public interface IDialogueReceiver
{
    public delegate void DialogueReceived(Dialogue dialogue);
    public event DialogueReceived OnDialogueReceived;
    public void ReceiveDialogue(Dialogue dialogue);
}
