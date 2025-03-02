using System.Collections;
using System.Collections.Generic;
using Mono.CompilerServices.SymbolWriter;
using UnityEngine;
public class MessageSource : MonoBehaviour
{
   public List<MessageData> messages;
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
   private IEnumerator PlayQueueCoroutine() {
      isPlaying = true;
      for (int i = 0; i < messages.Count; i++) {
         Play(messages[i]);
         yield return new WaitForSeconds(messages[i].messageDuration);
      }
      isPlaying = false;
   }
   public void Play(MessageData messageData) {
      AudioManager.Instance.PlayOneShot(messageData.messageEvent, transform.position);
   }
}
