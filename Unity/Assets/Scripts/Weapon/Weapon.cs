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
}
