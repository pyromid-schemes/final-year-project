using System;
using UnityEngine;
/*
 * @author Japeth Gurr (jarg2)
 * Class to handle Hand Tracked movement;
 * Includes validation and movement calculations
*/
namespace VirtualReality.MovementMethods
{
	public class RelativeToHand : IMovementMethod
	{

		public bool BeginMovement (SteamVR_Controller.Device controller)
		{
			return controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad);
		}

		public Vector3 RunMovement (GameObject playerHead, GameObject hand, GameObject player, SteamVR_Controller.Device controller)
		{
			// Work out where the tracked object is facing and move in that direction
			Vector2 rotation = MovementUtil.YRotationAsVector2(hand.transform.eulerAngles.y);
			return new Vector3 (MovementUtil.InterpolatePosition(rotation.x), 0, MovementUtil.InterpolatePosition(rotation.y));
		}

		public bool IdleMovement (SteamVR_Controller.Device controller, out Vector3 position)
		{
			position = Vector3.zero;
			return false;
		}
	}
}

