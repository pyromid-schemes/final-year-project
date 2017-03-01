using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private int damage;
    private bool blocked;
    private bool isColliding;


    public Weapon(int damage)
    {
        this.damage = damage;
        blocked = false;
        isColliding = false;
    }

    //TODO: set to false once AI and VR player has implemented the ability to toggle weapon hit boxes
    // Kept to true to for compatability issues until earlier statement is resolved
    void Start()
    {
        setWeaponIsActive(true);
    }

    public int GetDamage()
    {
        return damage;
    }

    void OnCollisionEnter(Collision other)
    {
        if (isColliding) return;
        isColliding = true;

        switch (other.collider.gameObject.tag)
        {
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

    void ApplyDamageToMonster(Collider other)
    {
        if (blocked)
        {
            blocked = false;
        }
        else
        {
            other.gameObject.GetComponent<IDamageable>().ApplyDamage(damage);
        }
    }

    void Update()
    {
        isColliding = false;
    }

    public void setWeaponIsActive(bool isActive)
    {
        GetComponent<BoxCollider>().enabled = isActive;
    }
}
