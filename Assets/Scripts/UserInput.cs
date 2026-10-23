using System;
using UnityEngine;
using UnityEngine.InputSystem;

using static UnityEngine.InputSystem.InputAction;

public class UserInput : MonoBehaviour
{
    public const string MOVE_ACTION_NAME = "Move";
    public const string JUMP_ACTION_NAME = "Jump";
    
    public static event Action<Vector2> OnMove;
    private InputAction moveAction;

    public static event Action OnJump;
    private InputAction jumpAction;

    [SerializeField] private PlayerInput playerInput;

    void Awake()
    {
        moveAction = playerInput.actions.FindAction(MOVE_ACTION_NAME);
        jumpAction = playerInput.actions.FindAction(JUMP_ACTION_NAME);
    }

    void OnEnable()
    {
        jumpAction.started += OnJumpStarted;
    }

    void OnDisable()
    {
        jumpAction.started -= OnJumpStarted;
    }

    void Update()
    {
        if(moveAction.IsInProgress()) OnMoveTriggered();
    }

    private void OnMoveTriggered() => OnMove?.Invoke(moveAction.ReadValue<Vector2>());

    private void OnJumpStarted(CallbackContext context) => OnJump?.Invoke();
}