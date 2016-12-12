using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private int damage;
    private bool blocked = false;

    public Weapon(int damage)
    {
        this.damage = damage;
    }

    public int GetDamage()
    {
        return damage;
    }

    void OnCollisionEnter(Collision other)
    {
        string tag = other.collider.gameObject.tag;
        switch (tag)
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
