using UnityEngine;
using System.Collections;
/*
 * @author Japeth Gurr (jarg2)
 * Script to handle functionality for Equipable objects;
 * Any GameObject with this script attatched will be treated as an equipable object 
*/
public class Equipable : MonoBehaviour {

    EquipableAudioManager AudioManager = null;
    SteamVR_Controller.Device HoldingHand = null;

    private float HapticPulseTimeRemainingS = 0;

    void Awake()
    {
        AudioManager = gameObject.GetComponent<EquipableAudioManager>();
    }

    void FixedUpdate()
    {
        if(HapticPulseTimeRemainingS > 0)
        {
            HapticPulseTimeRemainingS -= Time.deltaTime;
            HoldingHand.TriggerHapticPulse(500);
        }
        else
        {
            HapticPulseTimeRemainingS = 0;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        CheckIfPlayerCollision(col.collider);
    }

    void OnTriggerEnter(Collider other)
    {
        CheckIfPlayerCollision(other);
    }

    void CheckIfPlayerCollision(Collider other)
    {
        if (HoldingHand != null)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
            {
                HapticPulseTimeRemainingS = 0.1f;
                if (AudioManager) { AudioManager.PlayCollisionWith(other.gameObject.tag); }
            }
            else if(other.gameObject.tag == "Weapon" || other.gameObject.tag == "Shield")
            {
                HapticPulseTimeRemainingS = 0.1f;
                if (AudioManager) { AudioManager.PlayCollisionWith(other.gameObject.tag); }
            }
        }
    }

    public void EquippedByPlayer(SteamVR_Controller.Device device)
    {
        HoldingHand = device; 
    }

    public void UnequippedByPlayer()
    {
        HoldingHand = null;
    }
}
