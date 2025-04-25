using UnityEngine;

public class QuickCameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    public float lookSpeed = 2f; // Mouse look speed

    private float pitch = 0f; // Vertical angle
    private float yaw = 0f; // Horizontal angle

    void Update()
    {
        // Handle movement with WASD and QE
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime; // A/D or Left/Right
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime; // W/S or Up/Down
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.Q)) moveZ = -moveSpeed * Time.deltaTime; // Q (down)
        if (Input.GetKey(KeyCode.E)) moveZ = moveSpeed * Time.deltaTime;  // E (up)

        // Apply movement
        Vector3 move = transform.right * moveX + transform.up * moveZ + transform.forward * moveY;
        transform.Translate(move, Space.World);

        // Handle mouse look
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f); // Limit vertical look

        // Apply rotation
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }
}