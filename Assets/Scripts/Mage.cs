using UnityEngine;

public class Mage : Character
{
    [Header("Mage stats")]
    [SerializeField] private float maxHP = 80f;
    [SerializeField] private float damage = 35f;

    [Header("Mage settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firepoint;
    [SerializeField] private float projectileSpeed = 12f;
    [SerializeField] private float attackCooldown = 1.2f;

    private float nextAttackTime;

    private void Awake()
    {
        currentHp = maxHP;
    }

    public override void Attack()
    {
        if (Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + attackCooldown;

        // O flip e feito via localScale.x, por isso o sinal indica a direcao
        float direction = Mathf.Sign(transform.localScale.x);

        GameObject obj = Instantiate(projectilePrefab, firepoint.position, Quaternion.identity);

        if (obj.TryGetComponent(out Projectile projectile))
        {
            projectile.Launch(direction, projectileSpeed, damage, gameObject);
        }
    }
}