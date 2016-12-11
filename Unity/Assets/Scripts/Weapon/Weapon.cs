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

    void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        switch (tag)
        {
            case "Shield":
                blocked = true;
                break;
            case "Monster":
                ApplyDamageToMonster(other);
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
