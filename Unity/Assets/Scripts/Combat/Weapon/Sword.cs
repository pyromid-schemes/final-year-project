using UnityEngine;

public class Sword : Weapon2
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
        int damage = Random.Range(minDamage, maxDamage);
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

public abstract class Weapon2 : MonoBehaviour
{
    private bool isColliding;
    private bool blocked;

    public Weapon2()
    {
        isColliding = false;
        blocked = false;
    }

    //TODO: set to false once AI and VR player has implemented the ability to toggle weapon hit boxes
    // Kept to true to for compatability issues until earlier statement is resolved
    void Start()
    {
        setWeaponIsActive(true);
    }

    void OnCollisionEnter(Collision other)
    {
        if (isColliding) return;
        isColliding = true;

        switch (other.collider.gameObject.tag)
        {
            case "Weapon":
                blocked = true;
                break;
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

    private void ApplyDamageToMonster(Collider other)
    {
        if (blocked)
        {
            blocked = false;
        }
        else
        {
            other.gameObject.GetComponent<IDamageable>().ApplyDamage(GetDamage());
        }
    }

    public void setWeaponIsActive(bool isActive)
    {
        GetComponent<BoxCollider>().enabled = isActive;
    }

    void FixedUpdate()
    {
        isColliding = false;
    }

   public abstract int GetDamage();
}

