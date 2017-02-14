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

    void Start()
    {
        GetComponent<BoxCollider>().enabled = false;
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
        isColliding = false;
    }

    public void setWeaponIsActive(bool isActive)
    {
        GetComponent<BoxCollider>().enabled = isActive;
    }
}
