using UnityEngine;

public abstract class Character : MonoBehaviour, IDamageable
{
    protected float currentHp;

    public abstract void Attack();

    public virtual void TakeDamage(float amount)
    {
        currentHp -= amount;
        if (currentHp <= 0f)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
