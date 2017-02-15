using UnityEngine;
using System.Collections;
using VirtualReality.MovementMethods;

namespace VirtualReality
{
    [RequireComponent(typeof(SteamVR_TrackedObject))]
    public class Movement : MonoBehaviour
    {

        SteamVR_TrackedObject TrackedOBJ;
        SteamVR_Controller.Device Device;

        public GameObject Player;
        public GameObject PlayerHead;

        GameObject Hand;

		public IMovementMethod movementMethod;

        // Get this object
        void Awake()
        {
            TrackedOBJ = GetComponent<SteamVR_TrackedObject>();
            Hand = this.gameObject;
        }

        void FixedUpdate()
        {
            Device = SteamVR_Controller.Input((int)TrackedOBJ.index);

			if (movementMethod.BeginMovement (Device)) {
				Vector3 position = movementMethod.RunMovement (PlayerHead, Hand, Player, Device);
				Player.transform.Translate (position);
			} else {
				Vector3 position;
				if (movementMethod.IdleMovement (Device, out position)) {
					Player.transform.Translate (position);
				}
			}
        }
    }
}
