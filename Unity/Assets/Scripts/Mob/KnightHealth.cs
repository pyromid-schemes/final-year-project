﻿using UnityEngine;

public class KnightHealth : Damageable {

    private const int health = 2;
    GameObject self;

    public KnightHealth() : base(health)
    {
    }

    void Start()
    {
        self = this.gameObject;
    }
    void OnTriggerEnter()
    {
        Debug.Log(health);
        if (HealthIsZero())
        {
            OnDeath();
        }
    }
    /*
        void OnTriggerEnter(Collider other)
        {
            if (IsColliding())
            {
                return;
            }

            SetColliding(true);

            if (CollisionIsFromADamageSource(other.gameObject.tag))
            {
                int damage = other.GetComponent<Weapon>().GetDamage();
                ApplyDamage(damage);
            }


            if(HealthIsZero())
            {
                OnDeath();
            }
        }
    */


    protected override void OnDeath()
    {
        GameObject.DestroyObject(self);
    }
}
