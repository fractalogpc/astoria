using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class HitVisualHandler : MonoBehaviour
{
    [SerializeField] private Volume globalVolume;
    private PlayerHitEffect hit;
    private Vignette vignette;
    [SerializeField] private AnimationCurve red;
    [SerializeField] private AnimationCurve veggies;

}
