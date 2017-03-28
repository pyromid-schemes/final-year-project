using System;
using UnityEngine;

/*
 * @author: Kin Chung Cheng
 * Weapon class that handles collision detection and transfer of damage
 */
public abstract class Weapon : MonoBehaviour
{
    private bool isColliding;
    private bool blocked;

    private float TimeOfLastHit;
    public float DamageCooldownS = 0.1f;

    public Weapon()
    {
        isColliding = false;
        blocked = false;
    }

    // detects and indentifies the type of collision
    void OnCollisionEnter(Collision other)
    {
        if (CollisionIsValid(other))
        {
            isColliding = true;
            TimeOfLastHit = Time.time;

            switch (other.collider.gameObject.tag)
            {
                case "Weapon":
                    blocked = true;
                    break;
                case "Shield":
                    blocked = true;
                    break;
                case "Monster":
                    ApplyDamageToMonster(other.collider);
                    break;
                case "Player":
                    ApplyDamageToMonster(other.collider);
                    break;
                default:
                    break;
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        isColliding = false;
    }

    bool CollisionIsValid(Collision other)
    {
        return !isColliding && !other.gameObject.tag.Equals(transform.root.gameObject.tag);
    }

    void FixedUpdate()
    {
        if(TimeOfLastHit + DamageCooldownS < Time.time)
        {
            blocked = false;
        }
    }

    private void ApplyDamageToMonster(Collider other)
    {
        if(!blocked)
        {
            other.gameObject.GetComponent<IDamageable>().ApplyDamage(GetDamage());
        }
    }

    public void setWeaponIsActive(bool isActive)
    {
        foreach(Collider col in GetComponents<Collider>())
        {
            col.enabled = isActive;
        }
    }

    public abstract int GetDamage();
}
