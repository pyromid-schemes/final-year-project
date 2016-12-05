using UnityEngine;


public abstract class Damageable : MonoBehaviour {

    private bool colliding = false;
    private int health;

    public Damageable(int health)
    {
        this.health = health;
    }

    void Update()
    {
        colliding = false;
    }

    protected bool isColliding()
    {
        return colliding;
    }

    protected void SetColliding(bool colliding)
    {
        this.colliding = colliding;
    }

    protected bool CheckCollisionIsFromADamageSource(string damageSource)
    {
        Debug.Log(damageSource);
        if (damageSource.Equals("Weapon"))
        {
            return true;
        }
        return false;
    }

    protected void ApplyDamage(int damage)
    {
        health -= damage;
        Debug.Log("Health remaining " + health);
    }

    protected bool CheckHealthIsZero()
    {
        return health == 0;
    }

    protected abstract void OnDeath();
}
