using System;
using System.Collections.Generic;
using UnityEngine;
/*
    @author Jamie Redding (jgr2)
*/

namespace World
{
	public class PrefabMap : MonoBehaviour
	{
		// Prefabs
		public GameObject room1;
	    public GameObject room2;
	    public GameObject room3;
	    public GameObject room4;
	    public GameObject room5;
        public GameObject room6;
        public GameObject mobKnight;
		public GameObject mobSkellyCheng;

		private Dictionary<string, GameObject> map;
		
		void Awake ()
		{
			map = new Dictionary<string, GameObject> ();

			map.Add ("room1", room1);
			map.Add ("room2", room2);
			map.Add ("room3", room3);
			map.Add ("room4", room4);
			map.Add ("room5", room5);
            map.Add ("room6", room6);
            map.Add ("mobKnight", mobKnight);
			map.Add ("mobSkellyCheng", mobSkellyCheng);
		}

		public GameObject GetGameObject(string name)
		{
			GameObject obj = null;
			map.TryGetValue (name, out obj);
			return obj;
		}
	}
}

