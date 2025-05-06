using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class HitVisualHandler : MonoBehaviour
{
    [SerializeField] private Volume globalVolume;
    private PlayerHitEffect hit;
    private Vignette vignette;
    [SerializeField] private float t = 0;
    [SerializeField] private AnimationCurve red;
    [SerializeField] private AnimationCurve veggies;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (globalVolume.profile.TryGet<PlayerHitEffect>(out hit)) {
            Debug.Log("Why??");
        }
        if (globalVolume.profile.TryGet<Vignette>(out vignette))
            Debug.Log("Vignette"); 
    }

    // Update is called once per frame
    public void Hit()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            t = 0;
        }

        if (t < 1) {
            t += Time.deltaTime;
            hit.hit.value = red.Evaluate(t);
            vignette.intensity.value = veggies.Evaluate(t);
        }

        //hit.hit.value = 0;
    }
}
