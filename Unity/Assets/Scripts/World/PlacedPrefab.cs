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

		public override bool Equals(object value)
		{
			PlacedPrefab inst = value as PlacedPrefab;

			if (System.Object.ReferenceEquals(null, inst)) {
				return false;
			}

			return name.Equals (inst.name) && position.Equals (inst.position);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				const int HashingBase = (int) 2166136261;
				const int HashingMultiplier = 16777619;

				int hash = HashingBase;
				hash = (hash * HashingMultiplier) ^ (!System.Object.ReferenceEquals(null, name) ? name.GetHashCode() : 0);
				hash = (hash * HashingMultiplier) ^ (!System.Object.ReferenceEquals(null, position) ? position.GetHashCode() : 0);
				return hash;
			}
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

