public interface IDamageable
{
    void ApplyDamage(int damage);

    bool HealthIsZero();

    void OnZeroHealth();

    bool IsDead();

	int GetHealth();

	int GetMaxHealth();
}
