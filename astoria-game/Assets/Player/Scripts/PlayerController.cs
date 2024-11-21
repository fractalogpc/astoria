using System;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{

  public enum PlayerState
  {
    Default,
    Water
  }

  public class PlayerController : InputHandlerBase, IStartExecution //, ICharacterController
  {

    #region Variables

    private KinematicCharacterMotor _motor;

    [Header("Ground Movement")]
    [SerializeField] private float _maxGrounMoveSpeed = 10f;
    [SerializeField] private float _groundMovementSharpness = 15f;

    [Header("Air Movement")]
    [SerializeField] private float _maxAirMoveSpeed = 15f;
    [SerializeField] private float _airAccelerationSpeed = 15f;
    [SerializeField] private float _drag = 0.1f;

    [Header("Jumping")]
    [SerializeField] private bool _allowJumpingWhenSliding = false;
    [SerializeField] private float _jumpSpeed = 10f;
    [SerializeField] private float _jumpScalableForwardSpeed = 10f;
    [SerializeField] private float _jumpPreGroundingGraceTime = 0f;
    [SerializeField] private float _jumpPostGroundingGraceTime = 0f;

    [Header("Misc")]
    public List<Collider> IgnoreColliders = new List<Collider>();
    [SerializeField] private Transform _meshRoot;
    [SerializeField] private float _crouchedCapsulHeight = 1f;

    public PlayerState PlayerState { get; private set; }

    private Vector3 _moveVector;
    private Vector3 _lookVector;

    private Collider[] _probedColliders = new Collider[8];
    private RaycastHit[] _probedHits = new RaycastHit[8];
    private Vector3 _moveInputVector;
    private Vector3 _lookInputVector;
    private bool _jumpRequested = false;
    private bool _jumpConsumed = false;
    private bool _jumpedThisFrame = false;
    private float _timeSinceJumpRequested = Mathf.Infinity;
    private float _timeSinceLastAbleToJump = 0f;
    private Vector3 _internalVelocityAdd = Vector3.zero;
    private bool _shouldBeCrouching = false;
    private bool _isCrouching = false;

    private Vector3 _lastInnerNormal = Vector3.zero;
    private Vector3 _lastOuterNormal = Vector3.zero;

    #endregion

    private void Awake() {
      TransitionToState(PlayerState.Default);
    }

    public void InitializeStart()
    {
      _motor = GetComponent<KinematicCharacterMotor>();

      // _motor.CharacterController = this;
    }

    /// <summary>
    /// Handles state transitions and enter/exit callbacks
    /// </summary>
    /// <param name="newState"></param>
    public void TransitionToState(PlayerState newState)
    {
      if (PlayerState == newState) return;
      PlayerState tempState = PlayerState;
      OnStateExit(tempState, newState);
      PlayerState = newState;
      OnStateEnter(newState, tempState);
    }

    private void OnStateEnter(PlayerState newState, PlayerState oldState)
    {
      switch (newState)
      {
        case PlayerState.Default:
          break;
        case PlayerState.Water:
          break;
      }
    }

    private void OnStateExit(PlayerState oldState, PlayerState newState)
    {
      switch (oldState)
      {
        case PlayerState.Default:
          break;
        case PlayerState.Water:
          break;
      }
    }

    protected override void InitializeActionMap() {
      _actionMap = new Dictionary<InputAction, Action<InputAction.CallbackContext>>();

      RegisterAction(_inputActions.Player.Move, ctx => _moveVector = ctx.ReadValue<Vector2>(), () => _moveVector = Vector2.zero);
      RegisterAction(_inputActions.Player.Look, ctx => _lookVector = ctx.ReadValue<Vector2>(), () => _lookVector = Vector2.zero);
    }

  //   public void SetInputs()

  }
}