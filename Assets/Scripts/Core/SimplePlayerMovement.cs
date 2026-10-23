using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class SimplePlayerMovement : MonoBehaviour
{
    private const string MOVE_ACTION_NAME = "Player/Move";

    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D body;
    private InputAction moveAction;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        InputActionAsset actions = inputActions != null ? inputActions : InputSystem.actions;
        if (actions == null)
        {
            Debug.LogWarning("SimplePlayerMovement requires an InputActionAsset.", this);
            return;
        }

        moveAction = actions.FindAction(MOVE_ACTION_NAME, false);
        if (moveAction == null)
        {
            Debug.LogWarning($"SimplePlayerMovement could not find the {MOVE_ACTION_NAME} action.", this);
        }
    }

    private void FixedUpdate()
    {
        if (moveAction == null)
        {
            return;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        body.linearVelocity = new Vector2(input.x * moveSpeed, body.linearVelocity.y);
    }
}
