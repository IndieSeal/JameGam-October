using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Movement")]
    [SerializeField] private float movingSpeed = 5;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 20;

    [Header("Grounded Check")]
    [SerializeField] private float groundedRadius = 0.1f;
    [SerializeField] private LayerMask groundMask;

    void OnEnable()
    {
        UserInput.OnMove += OnMove;
        UserInput.OnJump += OnJump;
        UserInput.OnReset += OnReset;
    }

    void OnDisable()
    {
        UserInput.OnMove -= OnMove;
        UserInput.OnJump -= OnJump;
        UserInput.OnReset -= OnReset;
    }

    private void OnMove(Vector2 input)
    {
        if(!IsGrounded()) return;
        rb.linearVelocityX = input.x * movingSpeed * Time.deltaTime;
    }

    private void OnJump()
    {
        if(!IsGrounded()) return;
        rb.linearVelocityY = jumpForce;
    }

    private void OnReset()
    {
        if(IsGrounded() || rb.linearVelocity.sqrMagnitude > 0.1f) return;

        transform.eulerAngles = Vector2.zero;
    }

    private bool IsGrounded() => Physics2D.OverlapCircle(transform.position, groundedRadius, groundMask);

    void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, groundedRadius);
    }
}