using UnityEngine;
/*
 * @author Kin Chung Cheng
 * Lance class allows the Lance model to deal damage based on a range as well as
 * calculate if it can do critical damage by rolling a critical strike chance,
 * Weapon abstract class handles collision detection
 */
public class Lance : Weapon
{
    private const int maxDamage = 2;
    private const int minDamage = 1;

    private int critChance = 80;
    private int critMultiplier = 2;

    public Lance(): base()
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
            damage = damage * critMultiplier;
        }
        return damage;
    }

    private bool RollCriticalChance()
    {
        int roll = Random.Range(0, 100);
        if (roll < critChance)
        {
            print(roll<critChance);
            return true;
        }
        return false;
    }
}
