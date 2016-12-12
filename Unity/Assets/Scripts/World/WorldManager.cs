using UnityEngine;
using System.Collections.Generic;
using AI.Pathfinding;

namespace World
{
	public class WorldManager : MonoBehaviour, IWorldManager
	{
		public PrefabMap prefabs;
		public GameObject vrPlayer;
	    public Grid grid;

		private List<PositionalGameObject> objectSpawnQueue;
		private List<Mob> mobSpawnQueue;

		private HashSet<PlacedPrefab> gameWorld;
//		private HashSet<PlacedMob> mobs;

		void Start ()
		{
			objectSpawnQueue = new List<PositionalGameObject> ();
			mobSpawnQueue = new List<Mob> ();
			gameWorld = new HashSet<PlacedPrefab> ();
//			mobs = new HashSet<PlacedMob> ();
			AddPrefab ("room2", 0, 0);
		}
	
		void Update ()
		{
			for (int i = 0; i < objectSpawnQueue.Count; i++) {
				var obj = (GameObject)Instantiate (objectSpawnQueue [i].gameObj, objectSpawnQueue[i].position, Quaternion.identity);
				obj.SetActive (true);
				objectSpawnQueue.RemoveAt (i);
			}
			for (int i = 0; i < mobSpawnQueue.Count; i++) {
				var mob = (GameObject)Instantiate (mobSpawnQueue [i].pgo.gameObj, mobSpawnQueue[i].pgo.position, Quaternion.identity);
				mob.SetActive (true);
				Debug.Log ("spawned: " + mobSpawnQueue [i].name);
				mobSpawnQueue.RemoveAt (i);
//				mobs.Add (new PlacedMob (mobSpawnQueue [i].name, mobSpawnQueue [i].pgo.position, mobSpawnQueue [i].id, mob));
			}
		}

		public void AddPrefab (string objectId, int xPos, int zPos)
		{
			// TODO change where the name of the room is resolved
			GameObject obj = prefabs.GetGameObject (objectId);
			if (obj == null) {
				return;
			}

			Vector3 position = new Vector3 (xPos, 0, zPos);

			objectSpawnQueue.Add(new PositionalGameObject (obj, position));
		    grid.AddNodes(obj);
			gameWorld.Add (new PlacedPrefab (objectId, position));
		}

		public void SpawnMob (string objectId, float xPos, float zPos, int id)
		{
			GameObject obj = prefabs.GetGameObject (objectId);
			if (obj == null) {
				return;
			}

			Vector3 position = new Vector3 (xPos, 0, zPos);
			mobSpawnQueue.Add (new Mob (new PositionalGameObject (obj, position), id, objectId));
		}

		public HashSet<PlacedPrefab> GetGameWorld()
		{
			return gameWorld;
		}

		public Vector3 GetVRPosition()
		{
			return vrPlayer.transform.position;
		}
	}

	struct PositionalGameObject
	{
		public GameObject gameObj;
		public Vector3 position;

		public PositionalGameObject(GameObject g, Vector3 p)
		{
			gameObj = g;
			position = p;
		}
	}

	struct Mob
	{
		public PositionalGameObject pgo;
		public int id;
		public string name;

		public Mob(PositionalGameObject p, int i, string n)
		{
			pgo = p;
			id = i;
			name = n;
		}
	}
}