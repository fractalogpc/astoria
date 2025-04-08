using UnityEngine;

public class MoveUI : MonoBehaviour
{
    [SerializeField] private float _duration = 1f; // Duration of the move
    [SerializeField] private float _deltaHeight = 100f; // Distance to move the UI element
    [SerializeField] private AnimationCurve _curve; // Animation curve for the movement

    private RectTransform rectTransform; // Reference to the RectTransform component

    private void Awake()
    {
        // Get the RectTransform component attached to this GameObject
        rectTransform = GetComponent<RectTransform>();
    }

    public void MoveUIElement(bool direction)
    {
        // Start the coroutine to move the UI element
        StartCoroutine(MoveUICoroutine(direction));
    }

    private System.Collections.IEnumerator MoveUICoroutine(bool direction)
    {
        float elapsedTime = 0f;
        float startHeight = rectTransform.sizeDelta.y;
        float targetHeight = direction ? startHeight + _deltaHeight : startHeight - _deltaHeight;

        while (elapsedTime < _duration)
        {
            float t = elapsedTime / _duration;
            float curveValue = _curve.Evaluate(t);
            rectTransform.sizeDelta = new Vector2(1920, Mathf.Lerp(startHeight, targetHeight, curveValue));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.sizeDelta = new Vector2(1920, targetHeight); // Ensure the final position is set
    }
}
