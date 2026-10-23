using System;
using UnityEngine;
using UnityEngine.InputSystem;

using static UnityEngine.InputSystem.InputAction;

public class UserInput : MonoBehaviour
{
    public const string MOVE_ACTION_NAME = "Move";
    public const string JUMP_ACTION_NAME = "Jump";
    public const string RESET_ACTION_NAME = "Reset";
    
    public static event Action<Vector2> OnMove;
    private InputAction moveAction;

    public static event Action OnJump;
    private InputAction jumpAction;

    public static event Action OnReset;
    private InputAction resetAction;

    [SerializeField] private PlayerInput playerInput;

    void Awake()
    {
        moveAction = playerInput.actions.FindAction(MOVE_ACTION_NAME);
        jumpAction = playerInput.actions.FindAction(JUMP_ACTION_NAME);
        resetAction = playerInput.actions.FindAction(RESET_ACTION_NAME);
    }

    void OnEnable()
    {
        jumpAction.started += OnJumpStarted;
        resetAction.started += OnResetStarted;
    }

    void OnDisable()
    {
        jumpAction.started -= OnJumpStarted;
        resetAction.started -= OnResetStarted;
    }

    void Update()
    {
        if(moveAction.IsInProgress()) OnMoveTriggered();
    }

    private void OnMoveTriggered() => OnMove?.Invoke(moveAction.ReadValue<Vector2>());

    private void OnJumpStarted(CallbackContext context) => OnJump?.Invoke();

    private void OnResetStarted(CallbackContext context) => OnReset?.Invoke();
}