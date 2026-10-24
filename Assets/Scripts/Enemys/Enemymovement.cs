using UnityEngine;

//abstract base class for every enemy movement type 
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class Enemymovement : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField, Min(0f)] protected float patrolDistance = 3f;
    [Tooltip("Direction of the first movement.")]
    [SerializeField] protected bool startMovingRight = true;

    [Header("Ground check")]
    [Tooltip("Layers that count as ground and walls. If this is left on Nothing, grounded movement never works.")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField, Min(0.01f)] protected float groundCheckRadius = 0.1f;

    protected Rigidbody2D rb;
    protected Collider2D col;

    protected float startX;     //patrol centre, captured when the scene starts
    protected float direction;  // +1 = right, -1 = left

    //virtual so subclasses can add their own initialisation
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        startX = transform.position.x;
        direction = startMovingRight ? 1f : -1f;
    }

    //movement is physics-based, so it runs in FixedUpdate.
    private void FixedUpdate()
    {
        Move();
    }

    protected abstract void Move();

    //the ground check circle sits at the bottom of the collider
    protected bool IsGrounded()
    {
        Vector2 feet = new Vector2(col.bounds.center.x, col.bounds.min.y);
        return Physics2D.OverlapCircle(feet, groundCheckRadius, groundLayer) != null;
    }

    //turns around when the edge of the patrol area has been reached.
    protected void TurnAtPatrolLimits()
    {
        if (direction > 0f && transform.position.x >= startX + patrolDistance) direction = -1f;
        else if (direction < 0f && transform.position.x <= startX - patrolDistance) direction = 1f;
    }

    //if the enemy bumps sideways into a wall it turns around.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((groundLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Mathf.Abs(contact.normal.x) > 0.5f)
            {
                direction = Mathf.Sign(contact.normal.x); // Normal points away from the wall = new direction
                return;
            }
        }
    }

    //shows the patrol area in the Scene view when the enemy is selected
    private void OnDrawGizmosSelected()
    {
        float centreX = Application.isPlaying ? startX : transform.position.x;
        float y = transform.position.y;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(centreX - patrolDistance, y, 0f), new Vector3(centreX + patrolDistance, y, 0f));
        Gizmos.DrawWireSphere(new Vector3(centreX - patrolDistance, y, 0f), 0.15f);
        Gizmos.DrawWireSphere(new Vector3(centreX + patrolDistance, y, 0f), 0.15f);
    }
}
