using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private int maxHealth;
    private int health;

    private SpriteRenderer DamageOverlay;

    private bool isDead;

    public PlayerHealth()
    {
        maxHealth = 20;
        health = maxHealth;
        isDead = false;
    }

    void Start()
    {
        DamageOverlay = gameObject.GetComponent<SpriteRenderer>();
        FadeDamageOverlay();
    }

    void OnCollisionEnter()
    {
        if (HealthIsZero())
        {
            OnZeroHealth();
        }
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
        DamageOverlay.color = Color.red;
        Invoke("FadeDamageOverlay", 0.1f);
    }

    public void FadeDamageOverlay()
    {
        DamageOverlay.color = Color.clear;
    }

    public bool HealthIsZero()
    {
        return health <= 0;
    }

    public void OnZeroHealth()
    {
        //self.SetActive(false);
        isDead = true;
        print(isDead);
    }

    public bool IsDead()
    {
        return isDead;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
