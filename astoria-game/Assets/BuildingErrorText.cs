using UnityEngine;

public class BuildingErrorText : MonoBehaviour
{
    public AnimationCurve AnimationCurve;

    private CanvasGroup _canvasGroup;
    public float _fadeDuration = 0.5f;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 1f;

        timer = 0;
    }

    float timer;
    private void Update()
    {
        timer += Time.deltaTime;
        
        float t = timer / _fadeDuration;
        float alpha = AnimationCurve.Evaluate(t);
        _canvasGroup.alpha = alpha;
        if (timer > _fadeDuration)
        {
            Destroy(gameObject);
        }
    }

}
