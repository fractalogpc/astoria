using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWorldmodelAnimator : InputHandlerBase
{
    [SerializeField] private Animator _animator;
    
    [SerializeField] private Vector2 _movementInput;
    public bool IsMoving => _movementInput != Vector2.zero;

    public bool IsSprinting {
        get {
            if (!_sprintInputDown) return false;
            if (!IsMoving) return false;
            if (_movementInput.y < 0) return false;
            if (_movementInput.x != 0) return false;
            return true;
        }
    }
    private bool _sprintInputDown;
    protected override void InitializeActionMap() {
        RegisterAction(_inputActions.Player.Move, ctx => _movementInput = ctx.ReadValue<Vector2>(), () => _movementInput = Vector2.zero);
        RegisterAction(_inputActions.Player.Sprint, _ => _sprintInputDown = true, () => _sprintInputDown = false);
    }
    
    private void Update() {
        float inputX = _movementInput.x;
        float inputY = _movementInput.y;
        if (IsSprinting) {
            inputX = 0;
            inputY = 2;
        }
        _animator.SetFloat("InputX", inputX);
        _animator.SetFloat("InputY", inputY);
    }
}
