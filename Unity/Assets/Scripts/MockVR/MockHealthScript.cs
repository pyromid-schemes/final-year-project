using UnityEngine;

/*
 * @author Daniel Cheng
 * Health script for the mock VR player
 * This has a self kill key for testing of player health, to test this press k.
 * On zero health the player will be rendered inactive
 */
public class MockHealthScript : MonoBehaviour, IDamageable {
    private int maxHealth;
    private int health;

    private GameObject self;
    private bool isDead;

    public MockHealthScript()
    {
        maxHealth = 10;
        health = maxHealth;
        isDead = false;
    }

    void Start()
    {
        self = this.gameObject;
    }

    void Update()
    {
        if (UserPressKKey())
        {
            SelfHarmByFiveDamage();
        }

        if (HealthIsZero() && isDead == false)
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

    private bool UserPressKKey()
    {
        return Input.GetKeyDown(KeyCode.K);
    }

    private void SelfHarmByFiveDamage()
    {
        health -= 5;
    }
}
