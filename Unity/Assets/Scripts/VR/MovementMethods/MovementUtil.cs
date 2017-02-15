using System;
using UnityEngine;

namespace VirtualReality.MovementMethods
{
	public class MovementUtil
	{
        public static float speedMultiplier = 2f;

		public static Vector2 YRotationAsVector2(float angle)
		{
			return new Vector2(Mathf.Cos((90 - angle) * Mathf.Deg2Rad), Mathf.Sin((90 + angle) * Mathf.Deg2Rad));
		}

		public static float InterpolatePosition(float value)
		{
			return value * speedMultiplier * Time.deltaTime;
		}
	}
}

