using UnityEngine;
using System.Collections;
using System;

public class TestHit : Damageable {
   

    public TestHit() : base(2)
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (isColliding()) return;
        SetColliding(true);

        if (CheckCollisionIsFromADamageSource(other.gameObject.tag))
        {
            Debug.Log("Mob: Im hit");
            int damage = other.GetComponent<Weapon>().GetDamage();
            ApplyDamage(damage);
        }
        if(CheckHealthIsZero())
        {
            OnDeath();
        }
    }

    protected override void OnDeath()
    {
        Debug.Log(this.gameObject.ToString());     
        GameObject.Destroy(this.gameObject);
    }
}
