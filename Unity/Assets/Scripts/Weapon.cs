public abstract class Weapon{
    private int damage;

    public Weapon(int damage)
    {
        this.damage = damage;
    }

    public int GetTotalDamage()
    {
        return CalculateDamage();
    }

    protected abstract int CalculateDamage();
}
