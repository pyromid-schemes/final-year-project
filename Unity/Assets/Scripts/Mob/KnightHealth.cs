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
        Debug.Log("hello knight");
        self = this.gameObject;
    }

    void OnCollisionEnter()
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
