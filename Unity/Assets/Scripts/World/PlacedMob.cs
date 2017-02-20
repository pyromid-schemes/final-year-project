using System;
using UnityEngine;

namespace World
{
	public class PlacedMob
	{

		private GameObject gameObj;
		private int id;
		private string name;
		private bool killed;
		private Vector3 finalPos;

		public PlacedMob (string name, int id, GameObject gameObj)
		{
			this.gameObj = gameObj;
			this.id = id;
			this.name = name;
			killed = false;
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

		public bool HasBeenKilled()
		{
			return killed;
		}

		public void KillMob ()
		{
			killed = true;
			finalPos = gameObj.transform.position;
		}

		public Vector3 GetFinalPosition()
		{
			return finalPos;
		}
	}
}

