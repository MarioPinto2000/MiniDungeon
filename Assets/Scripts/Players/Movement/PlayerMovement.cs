
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;//empty child placed in the character feet
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;//layer that countsas ground

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;//must be a 1d axis float
    [SerializeField] private InputActionReference jumpAction;

    private float horizontal;//cached input (-1, 0 or 1)
    private bool isFacingRight = true;

    //for pause
    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
    }

    //for pause
    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    private void Update()
    {
        //input is read every frame
        horizontal = moveAction.action.ReadValue<float>();

        //jump only whe the bt is pressed this frame and the character is on the ground
        //WasPressedThisFrame avoids jumps in air
        if (jumpAction.action.WasPressedThisFrame() && IsGrounded())
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpingPower
            );
        }

        //releasing the button while still rising cuts the jump in half, tap = short jump, holp = full jump
        if (jumpAction.action.WasReleasedThisFrame() &&
            rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y * 0.5f
            );
        }

        Flip();
    }

    //physics runs at fixed timstep
    //y is preserved so gravity and jumps are not overwritten
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            horizontal * speed,
            rb.linearVelocity.y
        );
    }

    //true if circle at the feet touched any collider on gorundLayer
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        ) != null;
    }

    //mirrors the character by inverting the localScale.x
    //flips all children too
    private void Flip()
    {
        if ((isFacingRight && horizontal < 0f) ||
            (!isFacingRight && horizontal > 0f))
        {
            isFacingRight = !isFacingRight;

            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    //draws ground check for debbug
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );
    }
}
