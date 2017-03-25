using System;
using System.Collections;
using UnityEngine;
/*
 * @author Japeth Gurr and Daniel Cheng
 * Health script for the VR player.
 */
public class PlayerHealth : MonoBehaviour, IDamageable
{
    private int maxHealth;
    private int health;

    private SpriteRenderer DamageOverlay;

    private bool isDead;
    private float damageFadeDurationS;

    public PlayerHealth()
    {
        maxHealth = 20;
        health = maxHealth;
        isDead = false;
    }

    void Awake()
    {
        damageFadeDurationS = 0.5f;
        DamageOverlay = transform.Find("GUIDamageFlash").GetComponent<SpriteRenderer>();
        ClearDamageOverlay();
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
        StartCoroutine(FadeDamageOverlay());
    }

    private IEnumerator FadeDamageOverlay()
    {
        //for (float alpha = 1.0f; alpha > 0.0f; alpha -= (Time.deltaTime / damageFadeDurationS))
        //{
        //    Color newColor = DamageOverlay.color;
        //    newColor.a = alpha;
        //    DamageOverlay.color = newColor;
        //    yield return null;
        //}

        float opacity = 1.0f;
        while(opacity > 0f)
        {
            opacity -= (Time.deltaTime / damageFadeDurationS);
            Color newColor = DamageOverlay.color;
            newColor.a = opacity;
            DamageOverlay.color = newColor;
            yield return null;
        }
    }

    public void ClearDamageOverlay()
    {
        DamageOverlay.color = Color.clear;
    }

    public bool HealthIsZero()
    {
        return health <= 0;
    }

    public void OnZeroHealth()
    {
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
