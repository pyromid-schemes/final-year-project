using UnityEngine;


public abstract class Damageable : MonoBehaviour {

    private int health;
    private bool colliding;

    public Damageable(int health)
    {
        this.health = health;
        colliding = false;
    }

    void Update()
    {
        colliding = false;
    }

    protected bool CollisionIsFromADamageSource(string damageSource)
    {
        return damageSource.Equals("Weapon");
    }

    protected void ApplyDamage(int damage)
    {
        health -= damage;
    }

    protected bool HealthIsZero()
    {
        return health == 0;
    }

    protected abstract void OnDeath();

    protected bool IsColliding()
    {
        return colliding;
    }

    protected void SetColliding(bool colliding)
    {
        this.colliding = colliding;
    }
}
