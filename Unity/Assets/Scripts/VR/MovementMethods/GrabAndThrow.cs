using System;
using UnityEngine;

namespace VirtualReality.MovementMethods
{
	public class GrabAndThrow : IMovementMethod
	{
		bool GrabPressed = false;
		Vector3 GrabbedWorldPosition;
		Vector3 RelativeHeadPosition;
		Vector3 ThrowVelocity = Vector3.zero;
		float FrictionMultiplier; // TODO needs initialisation

		public bool BeginMovement (SteamVR_Controller.Device controller)
		{
			return controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad);
		}

		public Vector3 RunMovement (GameObject playerHead, GameObject hand, GameObject player, SteamVR_Controller.Device controller)
		{
			if (!GrabPressed)
			{
				GrabbedWorldPosition = hand.transform.position;
				RelativeHeadPosition = GetHeadPositionRelativeToPlayArea(playerHead, player);
				GrabPressed = true;
			}
			Vector3 distanceToGrabbedWorldPosition = hand.transform.position - GrabbedWorldPosition;
			GrabbedWorldPosition += GetHeadPositionRelativeToPlayArea(playerHead, player) - RelativeHeadPosition;

			// Apply movement
			RelativeHeadPosition = GetHeadPositionRelativeToPlayArea(playerHead, player);
			return new Vector3(distanceToGrabbedWorldPosition.x, 0, distanceToGrabbedWorldPosition.z);
		}
			
		Vector3 GetHeadPositionRelativeToPlayArea(GameObject playerHead, GameObject player)
		{
			return playerHead.transform.position - player.transform.position;
		}

		public bool IdleMovement (SteamVR_Controller.Device controller, out Vector3 position)
		{
			if (GrabPressed)
			{
				GrabPressed = false;
				ThrowVelocity = new Vector3(-controller.velocity.x, 0f, -controller.velocity.z);
			}
			if (ThrowVelocity != Vector3.zero)
			{
				position = new Vector3(MovementUtil.InterpolatePosition(ThrowVelocity.x), 0f, MovementUtil.InterpolatePosition(ThrowVelocity.z));
				ThrowVelocity *= FrictionMultiplier;
				return true;
			}
			position = Vector3.zero;
			return false; // TODO maybe an optional like thing or restructure idle-like movement?
		}
	}
}

