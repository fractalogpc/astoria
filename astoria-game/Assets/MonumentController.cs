using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MonumentController : Interactable
{

    public float timeToFinish = 10f;

    public UnityEvent OnInteractionFinished;

    public override void Interact()
    {
        base.Interact();

        StartCoroutine(StartInteraction());
    }

    private IEnumerator StartInteraction()
    {
        yield return new WaitForSeconds(timeToFinish);

        OnInteractionFinished?.Invoke();
    }
}
