using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spawn
{
	public class PrefabMap : MonoBehaviour
	{
		// Prefabs
		public GameObject room1;
		public GameObject room2;

		private Dictionary<string, GameObject> map;
		
		void Start ()
		{
			map = new Dictionary<string, GameObject> ();

			map.Add ("room-1", room1);
			map.Add ("room-2", room2);
		}

		public GameObject GetGameObject(string name)
		{
			GameObject obj = null;
			map.TryGetValue (name, out obj);
			return obj;
		}
	}
}

