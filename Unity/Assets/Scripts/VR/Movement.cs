using UnityEngine;
using System.Collections;

namespace VirtualReality
{
    [RequireComponent(typeof(SteamVR_TrackedObject))]
    public class Movement : MonoBehaviour
    {
        public enum MovementType
        {
            TrackedObjectGuided,
            TouchJoystick,
            PressJoystick,
            Teleportation,
            GrabAndThrow
        }

        public MovementType ChosenMovement;
        public float TouchpadDeadZone;
        public float Speed;
        public GameObject Player;
        public GameObject RotationTrackedObject;

        SteamVR_TrackedObject TrackedOBJ;
        SteamVR_Controller.Device Device;

        void Awake()
        {
            TrackedOBJ = GetComponent<SteamVR_TrackedObject>();
        }

        void FixedUpdate()
        {
            Device = SteamVR_Controller.Input((int)TrackedOBJ.index);
            switch (ChosenMovement)
            {
                case MovementType.TrackedObjectGuided:
                    if (Device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
                    {
                        // Work out where the tracked object is facing and move in that direction
                        Vector2 rotation = YRotationAsVector2(RotationTrackedObject.transform.eulerAngles.y);
                        Player.transform.Translate(
                            rotation.x * Speed * Time.deltaTime, 0f, rotation.y * Speed * Time.deltaTime);
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
                    break;
                case MovementType.Teleportation:
                    break;
            }
        }

        public void MovePlayerWithTouchpad()
        {
            // Work out where the tracked object is facing
            float currentAngleOfRotation = RotationTrackedObject.transform.eulerAngles.y;
            // Find the angle between where we pressed and above
            Vector2 touchPad = Device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
            float anglePressed = Vector2.Angle(new Vector2(0f, 1f), touchPad);
            // Work out and apply that direction as a vector
            float directionAngle = touchPad.x > 0 ? currentAngleOfRotation + anglePressed : currentAngleOfRotation - anglePressed;
            Vector2 movementVector = YRotationAsVector2(directionAngle);
            Player.transform.Translate(
               movementVector.x * Speed * Time.deltaTime, 0f,
               movementVector.y * Speed * Time.deltaTime);
        }

        Vector2 YRotationAsVector2(float angle)
        {
            return new Vector2(Mathf.Cos((90 - angle) * Mathf.Deg2Rad), Mathf.Sin((90 + angle) * Mathf.Deg2Rad));
        }
    }
}
