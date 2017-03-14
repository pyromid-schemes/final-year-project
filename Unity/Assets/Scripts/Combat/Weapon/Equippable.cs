using UnityEngine;
using System.Collections;

public class Equippable : MonoBehaviour {

    SteamVR_Controller.Device HoldingHand = null;

    private float PulseTimeRemaningS = 0;

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
                switch (other.gameObject.tag)
                {
                    case "Monster":
                        break;
                    case "Wall":
                        break;
                    case "Floor":
                        break;
                }
            }
            else
            {
                switch (other.gameObject.tag)
                {
                    case "Weapon":
                        PulseTimeRemaningS = 0.1f;
                        break;
                    case "Shield":
                        PulseTimeRemaningS = 0.1f;
                        break;
                }
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
