using System;
using UnityEngine;
/*
 * @author Japeth Gurr (jarg2)
 * Interface for all movement methods
*/
namespace VirtualReality.MovementMethods
{
	public interface IMovementMethod
	{
        // Validate start of movement
		bool BeginMovement (SteamVR_Controller.Device controller);

        // Calculate and return movement vector
		Vector3 RunMovement (GameObject playerHead, GameObject hand, GameObject player, SteamVR_Controller.Device controller);

        // If applicable, calculate and apply any idle movement
		bool IdleMovement (SteamVR_Controller.Device controller, out Vector3 position);
	}
}

