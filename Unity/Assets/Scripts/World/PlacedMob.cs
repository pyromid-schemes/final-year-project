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

