using UnityEngine;

//Abstract class shared by playables and enemys
//being an abstract claas only the subclasses Hero and Mage can be added to a GameObject
public abstract class Character : MonoBehaviour, IDamageable
{
    //max Hp , damage and attack cd are not defined here on pursose
    //each subclass declares its own values, so every character as different values
    protected float currentHp;

    public float CurrentHp => currentHp; //for hp ui
    public abstract float MaxHP { get; } //for hp ui

    public event System.Action Died;          //raised once when HP reaches 0
    private bool isDead;                      //makes sure Died is raised only once

    //every character must implement its own attack type
    //each subclas must implement its own cds inside this method

    public abstract void Attack();

    //reduce hp and triggers death when reach zero
    //virtual allows the subclasses to override it (e.g add more hp item)
    public virtual void TakeDamage(float amount)
    {
        currentHp -= amount;
        if (currentHp <= 0f)
        {
            Die();
        }
    }
  
    //virtual so override it for death animation for e.g
    protected virtual void Die()
    {
        if (isDead) return;                   //ignore extra hits after dying
        isDead = true;                       
        Died?.Invoke();                       
    }
}
