﻿using System;
using UnityEngine;

/*
 * @author Kin Chung Cheng
 * Health class for the prototype knight monster.
 */
public class KnightHealth : MonoBehaviour, IDamageable
{
	private int maxHealth;
	private int health;

    private GameObject self;
    private bool isDead;

    public KnightHealth()
    {
		maxHealth = 10;
		health = maxHealth;
        isDead = false;
    }

    void Start()
    {
        self = this.gameObject;
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
    }

    public bool HealthIsZero()
    {
        return health <= 0;
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

	public int GetHealth()
	{
		return health;
	}

	public int GetMaxHealth()
	{
		return maxHealth;
	}
}
