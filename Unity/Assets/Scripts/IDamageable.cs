using UnityEngine;
using System.Collections;

public interface IDamageable
{
    //Checks if the collision comes from a weapon
    bool CheckCollisionIsFromADamageSource(string damageSource);

    //Gets the damage from the weapon and applies to the player
    void ApplyDamage(int damage);
}

