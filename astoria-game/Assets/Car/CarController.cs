using System.Collections.Generic;
using UnityEngine;

public class CarController : InputHandlerBase
{

    public Wheel[] wheels;
    [SerializeField] private Transform _steeringTransform;

    [SerializeField] private float _steeringSensitivity = 0.05f;
    [SerializeField] private float _force = 50f;

    private float _steerAngle;
    private float SteerAngle { get => _steerAngle; set => _steerAngle = Mathf.Clamp(value, -1f, 1f); }
    private float _deltaSteer;

    private float _acceleration;

    [System.Serializable]
    public struct Wheel
    {
        public WheelCollider wheelCollider;
        public Transform wheelTransform;
        public bool isFrontWheel;
    }

    protected override void InitializeActionMap()
    {
        RegisterAction(_inputActions.Driving.Steer, value => _deltaSteer = value.ReadValue<float>(), () => _deltaSteer = 0f);
        RegisterAction(_inputActions.Driving.Accelerate, value => _acceleration = value.ReadValue<float>(), () => _acceleration = 0f);
    }

    private void FixedUpdate()

    {
        Steer();
        Accelerate();
        UpdateWheelPosition();
    }

    private void Steer()
    {
        float scaledSteer = _deltaSteer * _steeringSensitivity;

        if (scaledSteer == 0f)
        {
            if (SteerAngle > 0f)
            {
                if (SteerAngle - _steeringSensitivity < 0f)
                {
                    SteerAngle = 0f;
                }
                else
                {
                    SteerAngle -= _steeringSensitivity;
                }
            }
            else if (SteerAngle < 0f)
            {
                if (SteerAngle + _steeringSensitivity > 0f)
                {
                    SteerAngle = 0f;
                }
                else
                {
                    SteerAngle += _steeringSensitivity;
                }
            }
        }
        else
        {
            if (SteerAngle + scaledSteer > 1f)
            {
                SteerAngle = 1f;
            }
            else if (SteerAngle + scaledSteer < -1f)
            {
                SteerAngle = -1f;
            }
            else
            {
                SteerAngle += scaledSteer;
            }
        }

        foreach (var wheel in wheels)
        {
            if (wheel.isFrontWheel)
            {
                wheel.wheelCollider.steerAngle = SteerAngle * 45f;
            }
        }

        _steeringTransform.localRotation = Quaternion.Euler(0f, SteerAngle * 90f, 0f);
    }

    private void Accelerate()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.brakeTorque = 0f;
            // if (wheel.isFrontWheel)
            {
                wheel.wheelCollider.motorTorque = _acceleration * _force;
            }
        }
    }

    private void UpdateWheelPosition()
    {
        foreach (var wheel in wheels)
        {
            Vector3 position;
            Quaternion rotation;
            wheel.wheelCollider.GetWorldPose(out position, out rotation);

            wheel.wheelTransform.position = position;
            wheel.wheelTransform.rotation = rotation;
        }
    }
}
