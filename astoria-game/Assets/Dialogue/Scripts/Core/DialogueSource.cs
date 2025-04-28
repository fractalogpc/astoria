using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using UnityEngine;
public class DialogueSource : MonoBehaviour
{
   [Header("Use Timeline or outside scripts to play dialogue.")]
   private List<Dialogue> _dialogueInstances = new();
   private bool isPlaying;
   private bool _playedBefore;
   private Coroutine _playQueueCoroutine;

   public void StopPlayingOfData(DialogueData dialogueType) {
      List<Dialogue> dialoguesToStop = new();
      if (_dialogueInstances == null || _dialogueInstances.Count <= 0) return;
      foreach (Dialogue dialogue in _dialogueInstances) {
         if (dialogue.DialogueData == dialogueType) {
            dialoguesToStop.Add(dialogue);
         }
      }
      foreach (Dialogue dialogue in dialoguesToStop) {
         _dialogueInstances.Remove(dialogue);
         dialogue.End();
      }
   }
   
   public void StopPlayingAll(bool immediate = false) {
      if (_dialogueInstances == null || _dialogueInstances.Count <= 0) return;
      foreach (Dialogue dialogueInstance in _dialogueInstances) {
         dialogueInstance.SoundInstance.stop(immediate ? STOP_MODE.IMMEDIATE : STOP_MODE.ALLOWFADEOUT);
      }
      _dialogueInstances.Clear();
   }
   
   public void Play(DialogueData dialogueData) {
      Dialogue dialogue;
      if (dialogueData.AudioEvent.IsNull) {
         dialogue = dialogueData.CreateInstance(this);
         Debug.LogWarning($"Dialogue {dialogue.DialogueData.name} does not have an audio event. It will play no sound.");
      }
      else {
         dialogue = dialogueData.CreateInstance(this, AudioManager.Instance.PlayOneShotAttached(dialogueData.AudioEvent, gameObject));
      }
      dialogue.OnDialogueEnd += OnDialogueEnd;
      _dialogueInstances.Add(dialogue);
      if (dialogueData.RecieveRange > 0) {
         foreach (Collider col in Physics.OverlapSphere(transform.position, dialogueData.RecieveRange, dialogueData.RecieveLayerMask)) {
            col.GetComponentInChildren<IDialogueReceiver>().ReceiveDialogue(dialogue);
         }
      }
      else {
         // global range
         MonoBehaviour[] monoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
         List<IDialogueReceiver> receivers = monoBehaviours.OfType<IDialogueReceiver>().ToList();
         foreach (IDialogueReceiver receiver in receivers) {
            receiver.ReceiveDialogue(dialogue);
         }
      }
   }

   private void OnDialogueEnd(Dialogue dialogue) {
      dialogue.OnDialogueEnd -= OnDialogueEnd;
      _dialogueInstances.Remove(dialogue);
   }
   
   private void Update() {
      for (int i = _dialogueInstances.Count - 1; i >= 0; i--) {
         Dialogue dialogue = _dialogueInstances[i];
         dialogue.Tick(Time.deltaTime);
      }
   }

   private void OnDisable() {
      StopPlayingAll();
   }
}
