using System;
using UnityEngine;

namespace World
{
	public class PlacedPrefab
	{

		private string name;
		private Vector3 position;

		public PlacedPrefab (string name, Vector3 position)
		{
			this.name = name;
			this.position = position;
		}

		public string GetName()
		{
			return name;
		}

		public Vector3 GetPosition()
		{
			return position;
		}

	}
}

