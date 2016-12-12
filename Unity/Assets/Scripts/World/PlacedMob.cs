using System;
using UnityEngine;

namespace World
{
	public class PlacedMob
	{

		private GameObject gameObj;
		private int id;
		private string name;
		private Vector3 position;

		public PlacedMob (string name, Vector3 position, int id, GameObject gameObj)
		{
			this.gameObj = gameObj;
			this.id = id;
			this.name = name;
			this.position = position;
		}

		public override bool Equals(object value)
		{
			PlacedMob inst = value as PlacedMob;

			if (System.Object.ReferenceEquals(null, inst)) {
				return false;
			}

			return name.Equals (inst.name) && position.Equals (inst.position) && id.Equals (inst.id) && gameObj.Equals (inst.gameObj);
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
				hash = (hash * HashingMultiplier) ^ (!System.Object.ReferenceEquals(null, id) ? position.GetHashCode() : 0);
				hash = (hash * HashingMultiplier) ^ (!System.Object.ReferenceEquals(null, gameObj) ? position.GetHashCode() : 0);
				return hash;
			}
		}

		public int GetId()
		{
			return id;
		}

		public GameObject GetGameObject()
		{
			return gameObj;
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

