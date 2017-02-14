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
		private bool readyForKilling;

		public PlacedMob (string name, int id, GameObject gameObj)
		{
			this.gameObj = gameObj;
			this.id = id;
			this.name = name;
			killed = false;
			readyForKilling = false;
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

		public bool IsReadyForKilling()
		{
			return readyForKilling;
		}

		public void KillMob ()
		{
			if (!readyForKilling) {
				readyForKilling = true;
			} else {
				killed = true;
			}
		}
	}
}

