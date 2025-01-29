using UnityEngine;

/// <summary>
/// Adds recoil to the camera; viewmodel sway/anim is handled by CombatViewmodel.
/// </summary>
public class CombatCameraRecoil : MonoBehaviour
{

    private struct RecoilInstance
    {

        public AnimationCurve RecoilCurve;
        public float Duration;
        public float MagnitudeUpwards;
        public float MagnitudeHorizontal;
        public float MagnitudeBackwards;

    }

    [SerializeField] private Transform _recoilTransform;

    [SerializeField] private AnimationCurve _recoilCurve;


    public static CombatCameraRecoil Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public void ApplyRecoil(RecoilData recoilData) {
        
        
    }

}
