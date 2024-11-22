using UnityEngine;

public class TEMPORARYPlayerAnimationFromInput : MonoBehaviour
{

  private Animator _animator;

  void Start() {
    _animator = GetComponent<Animator>();
  }

  void Update() {
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");

    print("Horizontal: " + horizontal + " Vertical: " + vertical);
    _animator.SetFloat("Forward", vertical);
    _animator.SetFloat("Right", horizontal);
  }
}
