using System;
using UnityEngine;

namespace VirtualReality.MovementMethods
{
	public interface IMovementMethod
	{
		bool BeginMovement (SteamVR_Controller.Device controller);
		Vector3 RunMovement (GameObject playerHead, GameObject hand, GameObject player, SteamVR_Controller.Device controller);
		bool IdleMovement (SteamVR_Controller.Device controller, out Vector3 position);
	}
}

