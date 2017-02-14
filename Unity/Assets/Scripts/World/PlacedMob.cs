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
		private bool killMobOnWeb;

		public PlacedMob (string name, int id, GameObject gameObj)
		{
			this.gameObj = gameObj;
			this.id = id;
			this.name = name;
			killed = false;
			killMobOnWeb = false;
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

		public bool ShouldKillMobOnWeb()
		{
			return killMobOnWeb;
		}

		public void KillMob ()
		{
			if (!killMobOnWeb) {
				killMobOnWeb = true;
			} else {
				killed = true;
			}
		}
	}
}

