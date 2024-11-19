using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementNoMultiplayer : InputHandlerBase, IStartExecution
{
    [Header("General")]
    [SerializeField] private float _gravity = 20f;
    [SerializeField] private float _friction = 6f;

    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 6f;
    [SerializeField] private float _moveAcceleration = 14f;
    [SerializeField] private float _moveDeceleration = 10f;
    [SerializeField] private float _airAcceleration = 2f;
    [SerializeField] private float _airDeceleration = 2f;
    [SerializeField] private float _airControl = 0.3f;
    
    [SerializeField] private float _jumpSpeed = 8f;
    [SerializeField] private bool _holdJumpToBhop = false;

    private CharacterController _controller;

    private Vector3 _velocity;

    private Vector2 _inputDirection;
    private bool _jumpBuffered = false;

    public void InitializeStart() {
        _controller = GetComponent<CharacterController>();
    }

    protected override void InitializeActionMap() {
        _actionMap = new Dictionary<InputAction, Action<InputAction.CallbackContext>>();

        RegisterAction(_inputActions.Player.Move, ctx => _inputDirection = ctx.ReadValue<Vector2>(), () => _inputDirection = Vector2.zero);
        
        RegisterAction(_inputActions.Player.Jump, _ => _jumpBuffered = true, () => { if (_holdJumpToBhop) _jumpBuffered = false; });
    }

    private void Update() {
        if (_controller.isGrounded) {
            GroundMovement();
        } else {
            AirMovement();
        }

        _controller.Move(_velocity * Time.deltaTime);
    }

    private void GroundMovement() {

    }

    private void AirMovement() {

    }

}
