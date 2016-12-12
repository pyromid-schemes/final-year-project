using UnityEngine;

public class KnightHealth : Damageable
{

    private const int health = 2;
    private GameObject self;

    public KnightHealth() : base(health)
    {
    }

    void Start()
    {
        self = this.gameObject;
    }

    void OnTriggerEnter()
    { 
        if (HealthIsZero())
        {
            OnDeath();
        }
    }
 
    protected override void OnDeath()
    {
        GameObject.DestroyObject(self);
    }
}
