using UnityEngine;
using Unity.Cinemachine;

public class LerpSplinePosition : MonoBehaviour
{

    public SplineType splineType;
    public enum SplineType {
        CinemachineSplineCart,
        CinemachineSplineDolly,
    }

    public bool startOnStart = true;
    private bool start;
    public float maxTime;
    public float initValue;
    public float endValue;
    public AnimationCurve curve;
    private CinemachineSplineCart cart;
    private CinemachineSplineDolly dolly;

    void Start()
    {
        switch (splineType) {
            case SplineType.CinemachineSplineCart:
                cart = GetComponent<CinemachineSplineCart>();
                break;
            case SplineType.CinemachineSplineDolly:
                dolly = GetComponent<CinemachineSplineDolly>();
                break;
        }

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

        float t = Mathf.InverseLerp(0f, maxTime, timer);
        float curveValue = Mathf.Lerp(initValue, endValue, curve.Evaluate(t));

        switch (splineType) {
            case SplineType.CinemachineSplineCart:
                cart.SplinePosition = curveValue;
                break;
            case SplineType.CinemachineSplineDolly:
                dolly.SplineSettings.Position = curveValue;
                break;
        }
    }

    // 0.1610046

    public void StartLerp()
    {
        start = true;
        timer = 0f;

        switch (splineType) {
            case SplineType.CinemachineSplineCart:
                cart.SplinePosition = initValue;
                break;
            case SplineType.CinemachineSplineDolly:
                dolly.SplineSettings.Position = initValue;
                break;
        }
    }
}
