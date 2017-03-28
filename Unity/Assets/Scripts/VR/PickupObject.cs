using UnityEngine;
using System.Collections;
using System;
/*
 * @author Japeth Gurr (jarg2)
 * Script to handle picking up, handling, and releasing GameObjects;
 * One copy attatched to each hand
*/
namespace VirtualReality
{
    [RequireComponent(typeof(SteamVR_TrackedObject))]
    public class PickupObject : MonoBehaviour
    {
        public ItemMap ItemTransforms;

        SteamVR_TrackedObject TrackedObj;
        SteamVR_Controller.Device Device;

        GameObject EquippedObject = null;
        int DefaultChildren;

        // Get this object
        void Awake()
        {
            TrackedObj = GetComponent<SteamVR_TrackedObject>();
            DefaultChildren = transform.childCount;
        }

        void FixedUpdate()
        {
            Device = SteamVR_Controller.Input((int)TrackedObj.index);
            if(EquippedObject != null)
            {
                if (Device.GetTouchUp(SteamVR_Controller.ButtonMask.Grip))
                {
                    EquippedObject.transform.SetParent(null);
                    EquippedObject.GetComponent<Rigidbody>().isKinematic = false;
                    EquippedObject.GetComponent<Equippable>().UnequppedByPlayer();
                    TossObject(EquippedObject.GetComponent<Rigidbody>());
                    EquippedObject = null;
                }
            }
        }

        // Called during collisions
        void OnTriggerStay(Collider col)
        {
            if (HandIsEmpty() && AllowedToManipulate(col) && !AlreadyHeld(col))
            {
                ManipulateObject(col);
            }
        }

        bool HandIsEmpty()
        {
            return (transform.childCount <= DefaultChildren && EquippedObject == null);
        }

        bool AllowedToManipulate(Collider col)
        {
            return (col.gameObject.tag == "Weapon" || col.gameObject.tag == "Shield");
        }

        bool AlreadyHeld(Collider col)
        {
            return (col.transform.parent != null);
        }

        void ManipulateObject(Collider col)
        {
            // Pickup
            if (Device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
            {
                col.attachedRigidbody.isKinematic = true;
                col.gameObject.transform.SetParent(this.gameObject.transform);
                if (col.gameObject.GetComponent<Equippable>())
                {
                    EquippedObject = col.gameObject;
                    Transform equippable = ItemTransforms.GetDefaultTransform(col.name);
                    EquippedObject.transform.localRotation = equippable.rotation;
                    EquippedObject.transform.localPosition = equippable.position;

                    EquippedObject.GetComponent<Equippable>().EquippedByPlayer(Device);
                }
            }

            // Let Go
            else if (Device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                col.gameObject.transform.SetParent(null);
                col.attachedRigidbody.isKinematic = false;

                TossObject(col.attachedRigidbody);
            }
        }

        // Apply correct forces when letting go of objects
        void TossObject(Rigidbody rigidbody)
        {
            Transform origin = TrackedObj.origin ? TrackedObj.origin : TrackedObj.transform.parent;
            if (origin != null)
            {
                rigidbody.velocity = origin.TransformVector(Device.velocity);
                rigidbody.angularVelocity = origin.TransformVector(Device.angularVelocity);
            }
            else
            {
                rigidbody.velocity = Device.velocity;
                rigidbody.angularVelocity = Device.angularVelocity;
            }
        }
    }
}