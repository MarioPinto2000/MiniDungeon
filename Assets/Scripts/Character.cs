using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("Character Stats")]
    [SerializeField] protected float maxHP;
    [SerializeField] protected float damage;

    protected float currentHp;

    protected virtual void Awake()
    {
        currentHp = maxHP;
    }

    public abstract void Attack();
}
