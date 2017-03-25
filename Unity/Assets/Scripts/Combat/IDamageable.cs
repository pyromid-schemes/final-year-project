/*
 *@author Daniel Cheng
 * Base interface for all objects that should receive damage  
 */
public interface IDamageable
{   
    //open API that allows sources of damage to call this method to apply damage
    void ApplyDamage(int damage);

    //checks if the health is zero
    bool HealthIsZero();

    //execute method when health is at zero
    void OnZeroHealth();

    //open API for the network to monitor if the player /monster is dead
    bool IsDead();

    //open API for the network to monitor the health
	int GetHealth();

    //open API for the network to get the max health 
	int GetMaxHealth();
}
