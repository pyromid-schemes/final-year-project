using UnityEngine;
using System.Collections;
using System;

namespace VirtualReality
{
    [RequireComponent(typeof(SteamVR_TrackedObject))]
    public class PickupObject : MonoBehaviour
    {

        SteamVR_TrackedObject trackedOBJ;
        SteamVR_Controller.Device device;

        //public Transform projectile;

        void Awake()
        {
            trackedOBJ = GetComponent<SteamVR_TrackedObject>();
        }

        void FixedUpdate()
        {
            device = SteamVR_Controller.Input((int)trackedOBJ.index);

            //// Reset projectile to start location upon pressing the touchpad
            //if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
            //{
            //    projectile.transform.position = new Vector3(4.25f, 0f, 1.5f);
            //    projectile.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //    projectile.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            //}
        }

        // Called during collisions
        void OnTriggerStay(Collider col)
        {
            // Pickup
            if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
            {
                col.attachedRigidbody.isKinematic = true;
                col.gameObject.transform.SetParent(this.gameObject.transform);
            }

            // Let Go
            if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                col.gameObject.transform.SetParent(null);
                col.attachedRigidbody.isKinematic = false;

                TossObject(col.attachedRigidbody);
            }
        }

        // Apply correct forces
        private void TossObject(Rigidbody rigidbody)
        {
            Transform origin = trackedOBJ.origin ? trackedOBJ.origin : trackedOBJ.transform.parent;
            if (origin != null)
            {
                rigidbody.velocity = origin.TransformVector(device.velocity);
                rigidbody.angularVelocity = origin.TransformVector(device.angularVelocity);
            }
            else
            {
                rigidbody.velocity = device.velocity;
                rigidbody.angularVelocity = device.angularVelocity;
            }
        }
    }
}