using UnityEngine;
using System.Collections;

public class Equippable : MonoBehaviour {

    SteamVR_Controller.Device HoldingHand = null;

    void OnCollisionEnter(Collision col)
    {
        CheckIfPlayerCollision(col.collider);
    }

    void OnCollisionExit(Collision col)
    {
        CheckIfPlayerCollision(col.collider);
    }

    void OnTriggerEnter(Collider other)
    {
        CheckIfPlayerCollision(other);
    }

    void OnTriggerExit(Collider other)
    {
        CheckIfPlayerCollision(other);
    }

    void CheckIfPlayerCollision(Collider other)
    {
        if (HoldingHand != null)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
            {
                HoldingHand.TriggerHapticPulse(1000);
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
