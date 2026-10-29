using UnityEngine;

public class Mage : Character
{
    [Header("Mage stats")]
    [SerializeField] private float maxHP = 80f;
    [SerializeField] private float damage = 35f;
    [SerializeField, Min(0f)] private float invincibilityTime = 0.5f;

    [Header("Mage settings")]
    [SerializeField] private GameObject projectilePrefab; //prefab with Projectile + Rigidbody 2D + trigger collider
    [SerializeField] private Transform firepoint; //child object where projectile spawns
    [SerializeField] private float projectileSpeed = 12f;
    [SerializeField] private float attackCooldown = 1.2f;

    private float nextAttackTime;

    public override float MaxHP => maxHP; //for hp ui
    public override float InvincibilityTime => invincibilityTime; 

    //currentHp lives in the main class Character, but the maxhp lives here
    //every subclass has to initialize currentHp
    private void Awake()
    {
        currentHp = maxHP;
    }

    public override void Attack()
    {
        //CD attack
        if (Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + attackCooldown;

        //facing direction is stored in localScale.x
        //Math.Sign gives +1 if facing right and -1 when facing left
        float direction = Mathf.Sign(transform.localScale.x);

        GameObject obj = Instantiate(projectilePrefab, firepoint.position, Quaternion.identity);

        //The projectile is configured alter spawning so the same prefab works in both directions
        if (obj.TryGetComponent(out Projectile projectile))
        {
            //projectile can never hit mage
            projectile.Launch(direction, projectileSpeed, damage, gameObject);
        }
    }
}