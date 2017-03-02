using UnityEngine;

public class Sword : Weapon
{
    private const int maxDamage = 3;
    private const int minDamage = 1;

    private int chanceToCrit = 2;
    private int critMultiplier = 2;

    public Sword() : base(baseDamage)
    {
    }

    private int CalculateDamage()
    {
        int damage = Random.Range(minDamage, maxDamage);
        if (RollCriticalChance())
        {
            damage *= critMultiplier;
        }
        return damage;
    }

    private bool RollCriticalChance()
    {
        int percentage = Random.Range(0, 10);
        if (percentage < chanceToCrit)
        {
            return true;
        }
        else {
            return false;
        }
    }
}
