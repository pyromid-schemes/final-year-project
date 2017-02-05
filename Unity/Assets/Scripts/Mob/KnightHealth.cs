using System;
using UnityEngine;

public class KnightHealth : MonoBehaviour, IDamageable
{
    private int health;
    private GameObject self;

    public KnightHealth()
    {
        health = 3;
    }

    void Start()
    {
        self = this.gameObject;
    }

    void OnCollisionEnter()
    {
        Debug.Log(health);
        if (HealthIsZero())
        {
            OnDeath();
        }
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
    }

    public bool HealthIsZero()
    {
        return health == 0;
    }

    public void OnDeath()
    {
        self.SetActive(false);
    }
}
