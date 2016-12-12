using UnityEngine;


public abstract class Damageable : MonoBehaviour {
    private int health;

    public Damageable(int health)
    {
        this.health = health;
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
    }

    protected bool HealthIsZero()
    {
        return health == 0;
    }

    protected abstract void OnDeath();
}
