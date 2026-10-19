using UnityEngine;

public class Hero : Character
{
    [Header("Hero settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 1.5f;

    public override void Attack()
    {
        
    }
}
