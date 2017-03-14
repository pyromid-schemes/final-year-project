using UnityEngine;
using System.Collections;

public class Equippable : MonoBehaviour {

    EquippableAudioManager AudioManager = null;
    SteamVR_Controller.Device HoldingHand = null;

    private float PulseTimeRemaningS = 0;

    void Awake()
    {
        AudioManager = gameObject.GetComponent<EquippableAudioManager>();
    }

    void FixedUpdate()
    {
        if(PulseTimeRemaningS > 0)
        {
            PulseTimeRemaningS -= Time.deltaTime;
            HoldingHand.TriggerHapticPulse(500);
        }
        else
        {
            PulseTimeRemaningS = 0;
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
                PulseTimeRemaningS = 0.1f;
                if (AudioManager) { AudioManager.PlayCollisionWith(other.gameObject.tag); }
            }
            else if(other.gameObject.tag == "Weapon" || other.gameObject.tag == "Shield")
            {
                PulseTimeRemaningS = 0.1f;
                if (AudioManager) { AudioManager.PlayCollisionWith(other.gameObject.tag); }
            }
        }
    }

    public void EquippedByPlayer(SteamVR_Controller.Device device)
    {
        HoldingHand = device; 
    }

    public void UnequppedByPlayer()
    {
        HoldingHand = null;
    }
}
