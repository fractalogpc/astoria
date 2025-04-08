using UnityEngine;

public class CarController : MonoBehaviour
{
    // Wheel colliders
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    // Wheel models (for visual representation)
    public Transform frontLeftModel;
    public Transform frontRightModel;
    public Transform rearLeftModel;
    public Transform rearRightModel;

    // Car movement parameters
    public float maxMotorTorque = 1500f;  // Max torque for the motor
    public float maxSteeringAngle = 30f;  // Max steering angle
    public float brakeTorque = 3000f;     // Max brake force

    // Automatic Transmission Variables
    public int currentGear = 1;          // The current gear of the car (starting at 1 for 1st gear)
    public int totalGears = 5;           // Total number of gears (5 gears for example)
    public float gearRatio = 3.5f;       // Ratio for each gear (adjust based on your car model)
    public float maxEngineRPM = 6000f;   // Max RPM (depends on the car engine)
    public float minEngineRPM = 1000f;   // Min RPM before shifting down

    private float motorInput;            // Throttle input
    private float steeringInput;         // Steering input
    private float brakeInput;            // Braking input
    private float currentRPM;            // Current engine RPM
    private float previousSpeed;         // Store previous speed to detect shifts

    private void Update()
    {
        // Get player inputs
        motorInput = Input.GetAxis("Vertical");  // Forward/Backward throttle (W/S or Up/Down)
        steeringInput = Input.GetAxis("Horizontal");  // Steering (A/D or Left/Right)
        brakeInput = Mathf.Max(Input.GetKey(KeyCode.Space) ? 1f : 0f, -Input.GetAxis("Vertical"));  // Braking (Spacebar/S)

        // Update the car physics
        HandleMotor();
        HandleSteering();
        HandleBraking();
        UpdateWheelPositions();
        HandleAutomaticTransmission();
    }

    // Handle the motor torque (acceleration and deceleration)
    private void HandleMotor()
    {
        // Simulate RPM based on car speed and gear ratio
        currentRPM = Mathf.Clamp(GetCarSpeed() * gearRatio, 0, maxEngineRPM);
        
        // Shift gears based on RPM
        if (currentRPM >= maxEngineRPM * 0.95f) // If we're close to max RPM, shift up
        {
            ShiftUp();
        }
        else if (currentRPM <= minEngineRPM && currentGear > 1) // If we're low on RPM, shift down
        {
            ShiftDown();
        }

        // Apply motor torque based on the current gear
        frontLeftWheel.motorTorque = motorInput * maxMotorTorque * currentGear;
        frontRightWheel.motorTorque = motorInput * maxMotorTorque * currentGear;

        // Reverse motor torque if needed (for reverse driving)
        if (brakeInput > 0)
        {
            frontLeftWheel.motorTorque = 0;
            frontRightWheel.motorTorque = 0;
        }
    }

    // Handle the steering angle of the front wheels
    private void HandleSteering()
    {
        frontLeftWheel.steerAngle = steeringInput * maxSteeringAngle;
        frontRightWheel.steerAngle = steeringInput * maxSteeringAngle;
    }

    // Apply brakes to all wheels
    private void HandleBraking()
    {
        if (brakeInput > 0)
        {
            frontLeftWheel.brakeTorque = brakeTorque;
            frontRightWheel.brakeTorque = brakeTorque;
            rearLeftWheel.brakeTorque = brakeTorque;
            rearRightWheel.brakeTorque = brakeTorque;
        }
        else
        {
            frontLeftWheel.brakeTorque = 0;
            frontRightWheel.brakeTorque = 0;
            rearLeftWheel.brakeTorque = 0;
            rearRightWheel.brakeTorque = 0;
        }
    }

    // Update the position and rotation of wheel models to match WheelColliders
    private void UpdateWheelPositions()
    {
        UpdateWheelPosition(frontLeftWheel, frontLeftModel);
        UpdateWheelPosition(frontRightWheel, frontRightModel);
        UpdateWheelPosition(rearLeftWheel, rearLeftModel);
        UpdateWheelPosition(rearRightWheel, rearRightModel);
    }

    // Update a single wheel's position and rotation based on the WheelCollider
    private void UpdateWheelPosition(WheelCollider collider, Transform wheelModel)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        wheelModel.position = pos;
        wheelModel.rotation = rot;
    }

    // Shift the car up to the next gear
    private void ShiftUp()
    {
        if (currentGear < totalGears)
        {
            currentGear++;
            Debug.Log("Shifted up to gear " + currentGear);
        }
    }

    // Shift the car down to the previous gear
    private void ShiftDown()
    {
        if (currentGear > 1)
        {
            currentGear--;
            Debug.Log("Shifted down to gear " + currentGear);
        }
    }

    // Get the current speed of the car
    private float GetCarSpeed()
    {
        // Get the speed of the car from the Rigidbody's velocity
        return GetComponent<Rigidbody>().linearVelocity.magnitude * 3.6f; // Convert from m/s to km/h
    }

    // Handle automatic transmission logic
    private void HandleAutomaticTransmission()
    {
        // Shift up if the car has reached the maximum RPM for the current gear
        if (currentRPM >= maxEngineRPM * 0.95f && currentGear < totalGears)
        {
            ShiftUp();
        }

        // Shift down if the RPM is too low and we're not in first gear
        if (currentRPM <= minEngineRPM && currentGear > 1)
        {
            ShiftDown();
        }
    }
}