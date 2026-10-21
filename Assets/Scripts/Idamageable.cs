// Contract for anything that can receive damage
// Attackers (Hero melee, Projectile) only depend on this interface, never on a concrete class,
// so they can damage anything that implements it without knowing what it is.

public interface IDamageable
{
    void TakeDamage(float amount);
}