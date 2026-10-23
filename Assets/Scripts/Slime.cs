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

    private float nextAttackTime; //Time.time value alter wich the next hit is allowed

    //initialize currentHp
    private void Awake()
    {
        currentHp = maxHP;
    }

    //fires in the first frame of contact, cooldown stops the damage from being applied twice

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleContact(collision.collider);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleContact(collision.collider);
    }

    private void HandleContact(Collider2D other)
    {
        //layer check
        if ((playerLayer.value & (1 << other.gameObject.layer)) == 0) return;

        //cooldown logic
        if (Time.time < nextAttackTime) return;

        if (TryGetActiveCharacter(other, out Character target))
        {
            // The cooldown only starts when damage is actually dealt.
            nextAttackTime = Time.time + attackCooldown;
            Attack(target);
        }
    }

    //slime attack
    protected override void Attack(Character target)
    {
        target.TakeDamage(contactDamage);
    }
}
