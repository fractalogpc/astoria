using System.Collections;
using UnityEngine;

public class OtherCreditsManager : MonoBehaviour
{

    public Vector3 startPos;
    public Vector3 endPos;
    public float duration = 10f;

    public Transform creditsTransform;

    private void Start() {
        // Set the initial position of the credits to the start position
        creditsTransform.localPosition = startPos;
    }

    public void StartCredits() {
        creditsTransform.gameObject.SetActive(true); // Ensure credits are active before starting
        StartCoroutine(MoveCreditsCoroutine());
    }

    public void StopCredits() {
        creditsTransform.gameObject.SetActive(false); // Hide credits when stopping
        StopAllCoroutines(); // Stop all coroutines to prevent any ongoing credit movements
        creditsTransform.localPosition = startPos; // Reset position to start position
    }

    private IEnumerator MoveCreditsCoroutine() {
        yield return new WaitForSeconds(1f); // Delay before starting the credits
        float elapsedTime = 0f;
        while (elapsedTime < duration) {
            float t = elapsedTime / duration;
            creditsTransform.localPosition = Vector3.Lerp(startPos, endPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        creditsTransform.localPosition = endPos; // Ensure final position is set
    }
}
