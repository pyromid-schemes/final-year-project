using UnityEngine;
using System.Collections;

public class Equippable : MonoBehaviour {

    SteamVR_Controller.Device HoldingHand = null;

    void OnCollisionStay(Collision col)
    {
        CheckIfPlayerCollision(col.collider);
    }

    void OnTriggerStay(Collider other)
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
