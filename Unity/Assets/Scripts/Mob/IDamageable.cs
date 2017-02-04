public interface IDamageable
{
    void ApplyDamage(int damage);

    bool HealthIsZero();

    void OnDeath();
}
