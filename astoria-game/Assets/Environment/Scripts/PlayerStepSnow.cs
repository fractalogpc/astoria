using UnityEngine;

public class PlayerStepSnow : MonoBehaviour
{

  [SerializeField] private Transform _foot1;
  [SerializeField] private Transform _foot2;

  private void Update() {
    SetSnowDisplacement.Instance.DisplacePoint(_foot1.position);
    SetSnowDisplacement.Instance.DisplacePoint(_foot2.position);
  }

}