using System.Runtime.InteropServices;
using UnityEngine;

//basic enemy that hurts the player on contact
public class Slime : Enemy
{
    [Header("Slime stats")]
    [SerializeField] private float maxHP = 30f;
    [SerializeField] private float contactDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Slime settings")]
    [SerializeField] private LayerMask playerLayer; //Only objects that can be damaged

    //fields used by the overlap query that replaces the collision callbacks
    private Collider2D col;
    private ContactFilter2D playerFilter;
    private readonly Collider2D[] overlaps = new Collider2D[4];

    private float nextAttackTime; //Time.time value alter wich the next hit is allowed

    //initialize currentHp
    private void Awake()
    {
        currentHp = maxHP;

        //makes the query report only objects on the player layer.
        col = GetComponent<Collider2D>();
        playerFilter = new ContactFilter2D();
        playerFilter.SetLayerMask(playerLayer);
        playerFilter.useTriggers = true;
    }

    //fires in the first frame of contact, cooldown stops the damage from being applied twice

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    HandleContact(collision.collider);
    //}

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    HandleContact(collision.collider);
    //}

    // Collision callbacks are not sent between layers that do not collide, and the player must walk through the slime.
    private void FixedUpdate()
    {
        if (Time.time < nextAttackTime) return;

        Bounds bounds = col.bounds;
        int count = Physics2D.OverlapBox(bounds.center, bounds.size, 0f, playerFilter, overlaps);

        for (int i = 0; i < count; i++)
        {
            if (TryGetActiveCharacter(overlaps[i], out Character target))
            {
                nextAttackTime = Time.time + attackCooldown;
                Attack(target);
                return;
            }
        }
    }

    //private void HandleContact(Collider2D other)
    //{
    //    //layer check
    //    if ((playerLayer.value & (1 << other.gameObject.layer)) == 0) return;

    //    //cooldown logic
    //    if (Time.time < nextAttackTime) return;

    //    if (TryGetActiveCharacter(other, out Character target))
    //    {
    //        // The cooldown only starts when damage is actually dealt.
    //        nextAttackTime = Time.time + attackCooldown;
    //        Attack(target);
    //    }
    //}

    //slime attack
    protected override void Attack(Character target)
    {
        target.TakeDamage(contactDamage);
    }
}
