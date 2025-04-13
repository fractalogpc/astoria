using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CinematicCameraController : MonoBehaviour
{
    [Header("Movement")]
    public float _moveSpeed = 5f;
    public float acceleration = 10f;
    public float damping = 5f;

    [Header("Rotation")]
    public float rotationSpeed = 70f;
    public float rotationDamping = 5f;

    [Header("Zoom (Dolly)")]
    public float zoomSpeed = 10f;
    public float minFOV = 15f;
    public float maxFOV = 90f;

    [Header("Focus Target")]
    public Transform focusTarget;
    public float focusSmoothTime = 0.2f;

    private Vector3 currentVelocity;
    private Vector3 desiredPosition;
    private Quaternion desiredRotation;

    private Camera cam;
    private float desiredFOV;
    private float fovVelocity;

    void Start()
    {
        cam = GetComponent<Camera>();
        desiredPosition = transform.position;
        desiredRotation = transform.rotation;
        desiredFOV = cam.fieldOfView;
    }

    void Update()
    {
        HandleInput();
        UpdateCameraPosition();
        UpdateCameraRotation();
        UpdateCameraZoom();
        UpdateFocusTarget();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0)) {
            Cursor.lockState = CursorLockMode.Locked;
            // Should reset to center?
            Cursor.lockState = CursorLockMode.None;
        }
        
        // WASD movement
        Vector3 input = new Vector3(
            Input.GetAxis("Horizontal"),
            Input.GetKey(KeyCode.Space) ? 1 : Input.GetKey(KeyCode.C) ? -1 : 0,
            Input.GetAxis("Vertical")
        );
        
        float moveSpeed = _moveSpeed;
        
        if (Input.GetKey(KeyCode.LeftShift)) {
            moveSpeed = 20;
        }

        Vector3 moveDir = transform.TransformDirection(input.normalized);
        desiredPosition += moveDir * moveSpeed * Time.deltaTime;


        float yaw = 0;
        float pitch = 0;
        float roll = Input.GetKey(KeyCode.Q) ? rotationSpeed * Time.deltaTime : Input.GetKey(KeyCode.E) ? -rotationSpeed * Time.deltaTime : 0;
        // Rotation with arrow keys or mouse drag (Right Mouse)
        if (Input.GetMouseButton(0))
        {
            yaw = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            pitch = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        }
        desiredRotation = Quaternion.Euler(0, yaw, 0) * desiredRotation * Quaternion.Euler(pitch, 0, roll);
        
        if (Input.GetKey(KeyCode.R))
        {
            desiredRotation = Quaternion.Euler(cam.transform.eulerAngles.x, cam.transform.eulerAngles.y, 0);
        }


        
        // Zoom (FOV change)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            desiredFOV -= scroll * zoomSpeed * 100f * Time.deltaTime;
            desiredFOV = Mathf.Clamp(desiredFOV, minFOV, maxFOV);
        }
    }

    void UpdateCameraPosition()
    {
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, 1f / damping);
    }

    void UpdateCameraRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * rotationDamping);
    }

    void UpdateCameraZoom()
    {
        cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, desiredFOV, ref fovVelocity, 0.1f);
    }

    void UpdateFocusTarget()
    {
        if (focusTarget)
        {
            Vector3 direction = (focusTarget.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            desiredRotation = Quaternion.Slerp(desiredRotation, lookRotation, Time.deltaTime / focusSmoothTime);
        }
    }

    public void SetFocusTarget(Transform target)
    {
        focusTarget = target;
    }

    public void ClearFocusTarget()
    {
        focusTarget = null;
    }
}