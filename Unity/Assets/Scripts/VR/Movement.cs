using System;
using UnityEngine;
using VirtualReality.MovementMethods;
/*
 * @author Japeth Gurr (jarg2)
 * Script to handle Movement Input;
 * One copy attatched to each hand
*/
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

        public GameObject Player;
        public GameObject PlayerHead;

        public MovementType ChosenMovement;

        GameObject Hand;

        IMovementMethod MovementMethod;

        // Get this object
        void Awake()
        {
            TrackedOBJ = GetComponent<SteamVR_TrackedObject>();
            Hand = this.gameObject;
            switch (ChosenMovement)
            {
                case MovementType.HeadTracked: MovementMethod = new RelativeToPlayerHead(); break;
                case MovementType.HandTracked: MovementMethod = new RelativeToHand(); break;
                case MovementType.TouchJoystick: MovementMethod = new TouchTouchpad(); break;
                case MovementType.PressJoystick: MovementMethod = new PressTouchpad(); break;
                case MovementType.GrabAndThrow: MovementMethod = new GrabAndThrow(); break;
            }
        }

        void FixedUpdate()
        {
            Device = SteamVR_Controller.Input((int)TrackedOBJ.index);

            if (MovementMethod.BeginMovement(Device))
            {
                Vector3 position = MovementMethod.RunMovement(PlayerHead, Hand, Player, Device);
                if (MovementUtil.IsValidMove(PlayerHead.transform.position, position))
                {
                    Player.transform.Translate(position);
                }
            }
            else
            {
                Vector3 position;
                if (MovementMethod.IdleMovement(Device, out position))
                {
                    if (MovementUtil.IsValidMove(PlayerHead.transform.position, position))
                    {
                        Player.transform.Translate(position);
                    }
                }
            }
        }
    }
}
