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
                        Vector2 rotation = YRotationAsVector2(RotationTrackedObject.transform.eulerAngles.y);
                        Player.transform.Translate(
                            rotation.x * Speed * Time.deltaTime, 0f, rotation.y * Speed * Time.deltaTime);
                    }
                    break;
                case MovementType.TouchJoystick:
                    if (Device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
                    {
                        Debug.Log("Touchpad Touched");
                        Vector2 touchPad = Device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
                        Vector2 rotationVector = YRotationAsVector2(RotationTrackedObject.transform.eulerAngles.y);
                        Player.transform.Translate(
                            (touchPad.x - rotationVector.x) * Speed * Time.deltaTime, 0f,
                            (touchPad.y - rotationVector.y) * Speed * Time.deltaTime);
                    }
                    break;
                case MovementType.PressJoystick:
                    if (Device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
                    {
                        Debug.Log("Touchpad Pressed");
                        Vector2 touchPad = Device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
                        Vector2 rotationVector = YRotationAsVector2(RotationTrackedObject.transform.eulerAngles.y);
                        Player.transform.Translate(
                           (touchPad.x - rotationVector.x) * Speed * Time.deltaTime, 0f,
                           (touchPad.y - rotationVector.y) * Speed * Time.deltaTime);
                    }
                    break;
                case MovementType.GrabAndThrow:
                    break;
                case MovementType.Teleportation:
                    break;
            }
        }

        Vector2 YRotationAsVector2(float angle)
        {
            return new Vector2(Mathf.Cos((-90 - angle) * Mathf.Deg2Rad), Mathf.Sin((-90 + angle) * Mathf.Deg2Rad));
        }
    }
}
