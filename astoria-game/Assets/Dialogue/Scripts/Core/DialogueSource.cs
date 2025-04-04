using System;
using System.Collections;
using System.Collections.Generic;
using Mono.CompilerServices.SymbolWriter;
using UnityEngine;
public class DialogueSource : MonoBehaviour
{
   public List<DialogueData> dialogues;
   private List<Dialogue> _dialogueInstances;
   private bool isPlaying;
   private bool _playedBefore;
   private Coroutine _playQueueCoroutine;
   
   public void StopPlaying() {
      StopCoroutine(_playQueueCoroutine);
   }
   
   public void PlayQueue() {
      if (isPlaying || _playedBefore) {
         return;
      }
      _playedBefore = true;
      _playQueueCoroutine = StartCoroutine(PlayQueueCoroutine());
   }
   public void Play(DialogueData dialogueData) {
      Dialogue dialogue = dialogueData.CreateInstance();
      _dialogueInstances.Add(dialogue);
      if (!dialogue.DialogueData.AudioEvent.IsNull) {
         Debug.LogWarning($"Dialogue {dialogue.DialogueData.DialogueName} does not have an audio event. It will play no sound.");
         AudioManager.Instance.PlayOneShot(dialogueData.AudioEvent, transform.position);
      }
      foreach (Collider col in Physics.OverlapSphere(transform.position, dialogueData.RecieveRange, dialogueData.RecieveLayerMask)) {
         col.GetComponentInChildren<DialogueReceiver>().RecieveDialogue(dialogue);
      }
   }

   private void Update() {
      foreach (Dialogue dialogue in _dialogueInstances) {
         dialogue.Tick(Time.deltaTime);
         if (dialogue.DurationLeft <= 0) {
            _dialogueInstances.Remove(dialogue);
            break;
         }
      }
   }
   
   private IEnumerator PlayQueueCoroutine() {
      isPlaying = true;
      for (int i = 0; i < dialogues.Count; i++) {
         Play(dialogues[i]);
         yield return new WaitForSeconds(dialogues[i].Duration);
      }
      isPlaying = false;
   }
}
