using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private bool isColliding;
    private bool blocked;

    public Weapon()
    {
        isColliding = false;
        blocked = false;
    }

    void Start()
    {
        setWeaponIsActive(false);
    }

    void OnCollisionEnter(Collision other)
    {
        if (CollisionIsValid(other))
        {
            isColliding = true;

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

    bool CollisionIsValid(Collision other)
    {
        return !isColliding && !other.gameObject.tag.Equals(transform.root.gameObject.tag);
    }

    private void ApplyDamageToMonster(Collider other)
    {
        if (blocked)
        {
            blocked = false;
        }
        else
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

    void FixedUpdate()
    {
        isColliding = false;
    }

    public abstract int GetDamage();
}
