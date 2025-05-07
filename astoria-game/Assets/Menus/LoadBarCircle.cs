using UnityEngine;
using UnityEngine.UI;

public class LoadBarCircle : MonoBehaviour
{

    [SerializeField] private Image loadBarCircleImage;
    [SerializeField] private float circleSpeed = 1f;
    [SerializeField] private AnimationCurve loadBarCircleCurve;

    private float time = 0f;

    void Update()
    {
        // Need to do weird thing where like. we start with it at 0 then go to 1. Then flip the clockwise mode and slowly go back to 0. Et cetera.
        
        time += Time.deltaTime * circleSpeed;

        float value = loadBarCircleCurve.Evaluate(time);
        loadBarCircleImage.fillAmount = value;

        if (time >= 1f || time <= 0f)
        {
            circleSpeed *= -1f; // Reverse the speed to create a back and forth effect
            loadBarCircleImage.fillClockwise = !loadBarCircleImage.fillClockwise; // Reverse the clockwise direction
        }
    }
}
