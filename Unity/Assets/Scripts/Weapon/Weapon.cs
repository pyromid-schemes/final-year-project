using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private int damage;
    private bool blocked;
<<<<<<< HEAD
    private bool isColliding;
=======
    private bool isActive;
>>>>>>> master

    public Weapon(int damage)
    {
        this.damage = damage;
        blocked = false;
<<<<<<< HEAD
        isColliding = false;
    }


    //TODO: set to false once AI and VR player has implemented the ability to toggle weapon hit boxes
    // Kept to true to for compatability issues until earlier statement is resolved
    void Start()
    {
        setWeaponIsActive(true);
=======
        isActive = false;
>>>>>>> master
    }

    public int GetDamage()
    {
        return damage;
    }

    void OnCollisionEnter(Collision other)
    {
<<<<<<< HEAD
        if (isColliding) return;
        isColliding = true;
=======
        if (isActive) return;
        isActive = true;
>>>>>>> master

        switch (other.collider.gameObject.tag)
        {
            case "Shield":
                blocked = true;
                break;
            case "Monster":
                ApplyDamageToMonster(other.collider);
                break;
            default:
                break;
        }
    }

    void ApplyDamageToMonster(Collider other)
    {
        if (blocked)
        {
            blocked = false;
        }
        else
        {
            other.gameObject.SendMessage("ApplyDamage", damage);
        }
    }

    void Update()
    {
<<<<<<< HEAD
        isColliding = false;
    }

    public void setWeaponIsActive(bool isActive)
    {
        GetComponent<BoxCollider>().enabled = isActive;
=======
        isActive = false;
>>>>>>> master
    }
}
