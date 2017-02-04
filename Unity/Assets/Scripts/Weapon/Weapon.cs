using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private int damage;
    private bool blocked;

    public Weapon(int damage)
    {
        this.damage = damage;
        blocked = false;
    }

    public int GetDamage()
    {
        return damage;
    }

    void OnCollisionEnter(Collision other)
    {
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
}
