using UnityEngine;
using System.Collections;

namespace VirtualReality
{
    [RequireComponent(typeof(SteamVR_TrackedObject))]
    public class Movement : MonoBehaviour
    {
        public enum MovementType
        {
            HeadTracked,
            HandTracked,
            TouchJoystick,
            PressJoystick,
            Teleportation,
            GrabAndThrow
        }

        SteamVR_TrackedObject TrackedOBJ;
        SteamVR_Controller.Device Device;

        public MovementType ChosenMovement;
        public float SpeedMultiplier, FrictionMultiplier;
        public GameObject Player;
        public GameObject PlayerHead;

        GameObject Hand;
        bool GrabPressed = false;
        Vector3 GrabbedWorldPosition;
        Vector3 RelativeHeadPosition;
        Vector3 ThrowVelocity = Vector3.zero;

        // Get this object
        void Awake()
        {
            TrackedOBJ = GetComponent<SteamVR_TrackedObject>();
            Hand = this.gameObject;
        }

        void FixedUpdate()
        {
            Device = SteamVR_Controller.Input((int)TrackedOBJ.index);
            switch (ChosenMovement)
            {
                case MovementType.HeadTracked:
                    if (Device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
                    {
                        MovePlayerRelativeToTrackedObject(PlayerHead);
                    }
                    break;
                case MovementType.HandTracked:
                    if (Device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
                    {
                        MovePlayerRelativeToTrackedObject(Hand);
                    }
                    break;
                case MovementType.TouchJoystick:
                    if (Device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
                    {
                        MovePlayerWithTouchpad();
                    }
                    break;
                case MovementType.PressJoystick:
                    if (Device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
                    {
                        MovePlayerWithTouchpad();
                    }
                    break;
                case MovementType.GrabAndThrow:
                    if (Device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
                    {
                        MovePlayerWithGrab();
                    }
                    else
                    {
                        SlidePlayerAfterThrow();
                    }
                    break;
                case MovementType.Teleportation:
                    break;
            }
        }

        public void MovePlayer(float xVector, float zVector)
        {
            Player.transform.Translate(xVector * SpeedMultiplier * Time.deltaTime, 0f, zVector * SpeedMultiplier * Time.deltaTime);
        }

        public void MovePlayerRelativeToTrackedObject(GameObject obj)
        {
            // Work out where the tracked object is facing and move in that direction
            Vector2 rotation = YRotationAsVector2(obj.transform.eulerAngles.y);
            MovePlayer(rotation.x, rotation.y);
        }

        public void MovePlayerWithGrab()
        {
            if (!GrabPressed)
            {
                GrabbedWorldPosition = Hand.transform.position;
                RelativeHeadPosition = GetHeadPositionRelativeToPlayArea();
                GrabPressed = true;
            }
            Vector3 distanceToGrabbedWorldPosition = Hand.transform.position - GrabbedWorldPosition;
            GrabbedWorldPosition += GetHeadPositionRelativeToPlayArea() - RelativeHeadPosition;
            Player.transform.Translate(distanceToGrabbedWorldPosition.x, 0f, distanceToGrabbedWorldPosition.z);
            RelativeHeadPosition = GetHeadPositionRelativeToPlayArea();
        }

        public void SlidePlayerAfterThrow()
        {
            if (GrabPressed)
            {
                GrabPressed = false;
                ThrowVelocity = new Vector3(-Device.velocity.x, 0f, -Device.velocity.z);
            }
            if (ThrowVelocity != Vector3.zero)
            {
                MovePlayer(ThrowVelocity.x, ThrowVelocity.z);
                ThrowVelocity *= FrictionMultiplier;
            }
        }

        public void MovePlayerWithTouchpad()
        {
            // Work out where the tracked object is facing
            float currentAngleOfRotation = PlayerHead.transform.eulerAngles.y;
            // Find the angle between where we pressed and above
            Vector2 touchPad = Device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
            float anglePressed = Vector2.Angle(new Vector2(0f, 1f), touchPad);
            // Work out and apply that direction as a vector
            float directionAngle = touchPad.x > 0 ? currentAngleOfRotation + anglePressed : currentAngleOfRotation - anglePressed;
            Vector2 movementVector = YRotationAsVector2(directionAngle);
            MovePlayer(movementVector.x, movementVector.y);
        }

        Vector2 YRotationAsVector2(float angle)
        {
            return new Vector2(Mathf.Cos((90 - angle) * Mathf.Deg2Rad), Mathf.Sin((90 + angle) * Mathf.Deg2Rad));
        }

        public Vector3 GetHeadPositionRelativeToPlayArea()
        {
            return PlayerHead.transform.position - Player.transform.position;
        }
    }
}
