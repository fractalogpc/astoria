using UnityEngine;

public class SteeringWheelAnalyzer : MonoBehaviour
{
    
    [SerializeField] private Transform steeringWheelTransform;
    [SerializeField] private Transform carTransform;

    [SerializeField] private float amplitude = 1f;
    [SerializeField] private Vector2 steeringWheelRotationRange = new Vector2(-45f, 45f);
    [SerializeField] private int carRotationSamples = 20;

    private float[] carRotations;

    private Quaternion initialSteeringWheelRotation;

    private void Start()
    {
        // Store the initial rotation of the steering wheel
        initialSteeringWheelRotation = steeringWheelTransform.localRotation;
        
        // Initialize the car rotations array with the current rotation of the car
        carRotations = new float[carRotationSamples];
        for (int i = 0; i < carRotations.Length; i++)
        {
            carRotations[i] = carTransform.eulerAngles.y;
        }
    }

    private void FixedUpdate()
    {
        // Get current rotation of the car
        float carRotation = carTransform.eulerAngles.y;
        
        // Push the current rotation into the array
        for (int i = carRotations.Length - 1; i > 0; i--)
        {
            carRotations[i] = carRotations[i - 1];
        }
        carRotations[0] = carRotation;

        // Analyze angular velocity of the car
        float angularVelocity = 0f;
        for (int i = 0; i < carRotations.Length - 1; i++)
        {
            angularVelocity += carRotations[i] - carRotations[i + 1];
        }
        angularVelocity /= carRotations.Length - 1;

        // Calculate the steering wheel rotation based on the car's angular velocity
        float steeringWheelRotation = Mathf.Clamp(angularVelocity * amplitude, steeringWheelRotationRange.x, steeringWheelRotationRange.y);
        // Apply the rotation to the steering wheel
        steeringWheelTransform.localRotation = initialSteeringWheelRotation * Quaternion.Euler(0f, 0f, steeringWheelRotation);
    }

}
