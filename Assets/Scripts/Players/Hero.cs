using UnityEngine;

public class Hero : Character
{
    [Header("Hero stats")]
    [SerializeField] private float maxHP = 150f;
    [SerializeField] private float damage = 20f;

    [Header("Hero settings")]
    [SerializeField] private Transform attackPoint; //child object placed in front of this character
    [SerializeField] private float attackRange = 0.8f; //radious of the attack
    [SerializeField] private LayerMask enemyLayers; //only this layers can be hit
    [SerializeField] private float attackCooldown = 0.4f; 

    private float nextAttackTime;//Time.time value between attacks

    public override float MaxHP => maxHP; //for hp ui

    //IMPORTANT: currentHp lives in the main class Character, but the maxhp lives here
    //every subclass has to initialize currentHp
    private void Awake()
    {
        currentHp = maxHP;
    }

    public override void Attack()
    {
        //CD: ignores this call of still in cd
        if (Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + attackCooldown;

        //Finds every collider inside the attack range
        //attack point is a child of hero, so it follows and flips with hero
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayers
        );

        foreach (Collider2D hit in hits)
        {
            //hero can never hit itself
            if (hit.gameObject == gameObject) continue;

            //TryGetComponent avoids erros if the object cannot take damage
            if (hit.TryGetComponent(out IDamageable target))
            {
                target.TakeDamage(damage);
            }
        }
    }

    //Draws hit area in the sceane view if hero selected (debug feat)
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}