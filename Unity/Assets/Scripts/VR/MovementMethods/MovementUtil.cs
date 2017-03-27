using System;
using UnityEngine;
/*
 * @author Japeth Gurr (jarg2)
 * Class containing utility functions for movement methods
*/
namespace VirtualReality.MovementMethods
{
	public class MovementUtil
	{
        public static float SpeedMultiplier = 2f;
        public static float MovementBounds = 0.2f;
        public static LayerMask WorldCollisionMask = LayerMask.GetMask("World Geometry");

		public static Vector2 YRotationAsVector2(float angle)
		{
			return new Vector2(Mathf.Cos((90 - angle) * Mathf.Deg2Rad), Mathf.Sin((90 + angle) * Mathf.Deg2Rad));
		}

		public static float InterpolatePosition(float value)
		{
			return value * SpeedMultiplier * Time.deltaTime;
		}

        public static bool IsValidMove(Vector3 origin, Vector3 direction)
        {
            return !Physics.Raycast(origin, direction, MovementBounds, WorldCollisionMask);
        }
	}
}

