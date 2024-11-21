using UnityEngine;

public class CharacterCamera : MonoBehaviour
{
  public bool canLook = true;
	[SerializeField] private Camera _camera;
	[SerializeField] private float _sensitivity = 1;
	[SerializeField] private float _cameraXRotation = 0;
	[HideInInspector] public Transform Transform;

  private void Awake() {
    Transform = transform;
  }

	private void Look(Vector2 _mouseInput) {
		Vector2 mouseInput = _mouseInput * _sensitivity;
		Transform.transform.Rotate(Vector3.up, mouseInput.x);
		_cameraXRotation -= mouseInput.y;
		if (_cameraXRotation > 90) _cameraXRotation = 90;
		if (_cameraXRotation < -90) _cameraXRotation = -90;
		_camera.transform.localRotation = Quaternion.Euler(_cameraXRotation, 0, 0);
	}

  public void UpdateWithInput(Vector2 input) {
    if (canLook) Look(input);
  }

}
