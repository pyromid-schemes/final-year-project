using System;
using UnityEngine;
/*
    @author Jamie Redding (jgr2)
*/

namespace World
{
	public class PlacedPrefab
	{

		private string name;
		private Vector3 position;
		private Quaternion rotation;

		public PlacedPrefab (string name, Vector3 position, Quaternion rotation)
		{
			this.name = name;
			this.position = position;
			this.rotation = rotation;
		}

		public string GetName()
		{
			return name;
		}

		public Vector3 GetPosition()
		{
			return position;
		}

		public Quaternion GetRotation()
		{
			return rotation;
		}

	}
}

