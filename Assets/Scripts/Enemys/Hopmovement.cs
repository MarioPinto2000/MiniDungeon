using UnityEngine;

//patrols left and right by hopping
public class Hopmovement : Enemymovement
{
    [Header("Hop movement")]
    [SerializeField, Min(0f)] private float hopForce = 6f;
    [SerializeField, Min(0f)] private float hopSpeed = 3f;
    [SerializeField, Min(0f)] private float timeBetweenHops = 1f;

    private float hopTimer; //counts down while the enemy is on the ground

    protected override void Awake()
    {
        base.Awake(); // Sets up rb, col, startX and direction
        hopTimer = timeBetweenHops;
    }

    protected override void Move()
    {
        //only act when standing on the ground and not moving upward.
        if (!IsGrounded() || rb.linearVelocity.y > 0.01f) return;

        //stop sliding horizontally so the enemy rests between hops.
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        hopTimer -= Time.fixedDeltaTime;
        if (hopTimer <= 0f)
        {
            Hop();
        }
    }

    private void Hop()
    {
        TurnAtPatrolLimits();

        //gravity brings the enemy back down, which creates the hop arc.
        rb.linearVelocity = new Vector2(direction * hopSpeed, hopForce);

        hopTimer = timeBetweenHops;
    }
}
