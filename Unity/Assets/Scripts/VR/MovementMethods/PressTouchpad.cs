using System;
using UnityEngine;

namespace VirtualReality.MovementMethods
{
	public class PressTouchpad : IMovementMethod
	{

		public bool BeginMovement (SteamVR_Controller.Device controller)
		{
			return controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad);
		}

		public Vector3 RunMovement (GameObject playerHead, GameObject hand, GameObject player, SteamVR_Controller.Device controller)
		{
			// Work out where the tracked object is facing
			float currentAngleOfRotation = playerHead.transform.eulerAngles.y;
			// Find the angle between where we pressed and above
			Vector2 touchPad = controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
			float anglePressed = Vector2.Angle(new Vector2(0f, 1f), touchPad);
			// Work out and apply that direction as a vector
			float directionAngle = touchPad.x > 0 ? currentAngleOfRotation + anglePressed : currentAngleOfRotation - anglePressed;
			Vector2 movementVector = MovementUtil.YRotationAsVector2(directionAngle);

			return new Vector3 (MovementUtil.InterpolatePosition(movementVector.x), 0, MovementUtil.InterpolatePosition(movementVector.y));
		}

		public bool IdleMovement (SteamVR_Controller.Device controller, out Vector3 position)
		{
			position = Vector3.zero;
			return false;
		}
	}
}

