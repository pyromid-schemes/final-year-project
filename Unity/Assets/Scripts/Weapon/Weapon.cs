using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private int damage;

    public Weapon(int damage)
    {
        this.damage = damage;
    }

    public int GetDamage()
    {
        return damage;
    }

    void OnTriggerEnter(Collider other) {
        bool blocked = false;

        if (other.gameObject.tag.Equals("Shield"))
        {
            blocked = true;
        }

        if (other.gameObject.tag.Equals("Monster"))
        {
            int totalDamage = 0;
            if (!blocked)
            {
                totalDamage += damage;
            }
            
            other.gameObject.SendMessage("ApplyDamage",totalDamage);
        }
    }
}
