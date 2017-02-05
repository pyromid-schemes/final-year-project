using System;
using UnityEngine;

public class KnightHealth : MonoBehaviour, IDamageable
{
    private int health;
    private GameObject self;
    private bool isDead;

    public KnightHealth()
    {
        health = 3;
        isDead = false;
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
            OnZeroHealth();
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

    public void OnZeroHealth()
    {
        self.SetActive(false);
        isDead = true;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
