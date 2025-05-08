using UnityEngine;
using Unity.Cinemachine;

public class LerpSplinePosition : MonoBehaviour
{
    public bool startOnStart = true;
    private bool start;
    public float maxTime;
    public AnimationCurve curve;
    private CinemachineSplineCart cart;

    void Start()
    {
        cart = GetComponent<CinemachineSplineCart>();

        if (startOnStart) {
            StartLerp();
        }
    }

    float timer = 0f;
    void Update()
    {
        if (!start) return;

        timer += Time.deltaTime;
        if (timer > maxTime) {
            timer = maxTime;
            start = false;
        }

        float t = timer / maxTime;
        float curveValue = curve.Evaluate(t);
        cart.SplinePosition = Mathf.Lerp(0f, 1f, curveValue);
    }

    public void StartLerp()
    {
        start = true;
        timer = 0f;
        cart.SplinePosition = 0f;
    }
}
