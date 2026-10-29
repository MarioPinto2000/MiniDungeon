using UnityEngine;

//abstract class shared by every enemy
// IDamageable implemented so hero and mage projectile can hurt
public abstract class Enemy : MonoBehaviour, IDamageable
{
    //stats are defined in the subclasses

    protected float currentHp;

    //every enemy type defines how it attacks a target
    //being the target the player

    protected abstract void Attack(Character target);
    
    //reduces hp and triggers death when currentHp = 0

    public virtual void TakeDamage(float amount)
    {
        currentHp -= amount;
        if (TryGetComponent(out DamageFlash flash)) flash.Flash(); //damage flash
        if (currentHp <= 0)
        {
            Die();
        }
    }

    //death behaviour
    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    //the player has both hero and mage at the same time but only one is enable at a time

    protected static bool TryGetActiveCharacter(Collider2D other, out Character activeCharacter)
    {
        foreach (Character character in other.GetComponentsInParent<Character>()) 
        {
            if (character.enabled)
            {
                activeCharacter = character;
                return true;
            }
        }

        activeCharacter = null;
        return false;
    }
}
