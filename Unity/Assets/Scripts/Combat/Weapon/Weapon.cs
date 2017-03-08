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

    //TODO: set to false once AI and VR player has implemented the ability to toggle weapon hit boxes
    // Kept to true to for compatability issues until earlier statement is resolved
    void Start()
    {
        setWeaponIsActive(true);
    }

    void OnCollisionEnter(Collision other)
    {
        if (isColliding) return;
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
                print("hit");
                ApplyDamageToMonster(other.collider);
                break;
            case "Player":
                ApplyDamageToMonster(other.collider);
                break;
            default:
                break;
        }
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
        GetComponent<BoxCollider>().enabled = isActive;
    }

    void FixedUpdate()
    {
        isColliding = false;
    }

    public abstract int GetDamage();
}
