using UnityEngine;

public class Sword : Weapon
{
    private const int maxDamage = 3;
    private const int minDamage = 1;

    private int critChance = 20;
    private int critMultiplier = 2;

    public Sword() : base()
    {
    }

    public override int GetDamage()
    {
        return CalculateDamage();
    }

    private int CalculateDamage()
    {
        int damage = Random.Range(minDamage, maxDamage + 1);
        if (RollCriticalChance())
        {
            damage *= critMultiplier;
        }
        return damage;
    }

    private bool RollCriticalChance()
    {
        int roll = Random.Range(0, 100);
        if (roll < critChance)
        {
            return true;
        }
        return false;
    }
}