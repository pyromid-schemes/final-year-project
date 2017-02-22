using UnityEngine;

public class SkeletonHealth : MonoBehaviour, IDamageable
{
	private int maxHealth;
    private int health;
    private GameObject self;
    private bool isDead;

    public SkeletonHealth()
    {
		maxHealth = 5;
		health = maxHealth;
        isDead = false;
    }

    void Start()
    {
        self = this.gameObject;
    }

    void OnCollisionEnter()
    {
        Debug.Log(health);
        if (HealthIsZero())
        {
            OnZeroHealth();
        }
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
    }

    public bool HealthIsZero()
    {
        return health <= 0;
    }

    public void OnZeroHealth()
    {
        self.SetActive(false);
        isDead = true;
    }

    public bool IsDead()
    {
        return isDead;
    }

	public int GetHealth()
	{
		return health;
	}

	public int GetMaxHealth()
	{
		return maxHealth;
	}
}
