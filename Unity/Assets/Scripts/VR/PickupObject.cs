using UnityEngine;
using System.Collections;
using System;

namespace VirtualReality
{
    [RequireComponent(typeof(SteamVR_TrackedObject))]
    public class PickupObject : MonoBehaviour
    {
        public ItemMap itemMap;
        SteamVR_TrackedObject trackedOBJ;
        SteamVR_Controller.Device device;

        GameObject equipedObject = null;

        // Get this object
        void Awake()
        {
            trackedOBJ = GetComponent<SteamVR_TrackedObject>();
        }

        void FixedUpdate()
        {
            device = SteamVR_Controller.Input((int)trackedOBJ.index);
            if(equipedObject != null)
            {
                if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Grip))
                {
                    equipedObject.transform.SetParent(null);
                    equipedObject.GetComponent<Rigidbody>().isKinematic = false;
                    equipedObject.GetComponent<Equippable>().UnequppedByPlayer();
                    TossObject(equipedObject.GetComponent<Rigidbody>());
                    equipedObject = null;
                }
            }
        }

        // Called during collisions
        void OnTriggerStay(Collider col)
        {
            // TODO: Check if i'm already holding something (Count children?)
            if (equipedObject == null)
            {
                ManipulateObject(col);
            }
        }

        private void ManipulateObject(Collider col)
        {
            // Pickup
            if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
            {
                // TODO: Check what i'm picking up is valid
                col.attachedRigidbody.isKinematic = true;
                col.gameObject.transform.SetParent(this.gameObject.transform);
                if (col.gameObject.GetComponent<Equippable>())
                {
                    equipedObject = col.gameObject;
                    Transform test = itemMap.GetDefaultTransform(col.name);
                    equipedObject.transform.localRotation = test.rotation;
                    equipedObject.transform.localPosition = test.position;

                    equipedObject.GetComponent<Equippable>().EquippedByPlayer(device);
                }
            }

            // Let Go
            else if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                col.gameObject.transform.SetParent(null);
                col.attachedRigidbody.isKinematic = false;

                TossObject(col.attachedRigidbody);
            }
        }

        // Apply correct forces when letting go of objects
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