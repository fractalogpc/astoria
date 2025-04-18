using UnityEngine;

public class CaptiveCameraBounce : MonoBehaviour
{

    [SerializeField] private float bounceHeight = 1.0f;
    [SerializeField] private float bounceSpeed = 1.0f;

    private Vector3 originalPosition;

    void Awake()
    {
        // Store the original position of the camera
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        // Calculate the new position
        float newY = originalPosition.y + Mathf.PerlinNoise(Time.time * bounceSpeed, 0) * bounceHeight;

        // Update the camera's position
        transform.localPosition = new Vector3(originalPosition.x, newY, originalPosition.z);
    }
}
