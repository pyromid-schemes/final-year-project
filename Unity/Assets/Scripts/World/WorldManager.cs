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

		private List<Room> roomSpawnQueue;
		private List<Mob> mobSpawnQueue;

		private HashSet<PlacedPrefab> gameWorld;
		private HashSet<PlacedMob> mobs;

		void Start ()
		{
			roomSpawnQueue = new List<Room> ();
			mobSpawnQueue = new List<Mob> ();
			gameWorld = new HashSet<PlacedPrefab> ();
			mobs = new HashSet<PlacedMob> ();
			AddPrefab ("room2", 0, 0);
		}
	
		void Update ()
		{
			for (int i = 0; i < roomSpawnQueue.Count; i++) {
				var obj = (GameObject)Instantiate (roomSpawnQueue [i].gameObj, roomSpawnQueue[i].position, Quaternion.identity);
				obj.SetActive (true);
				roomSpawnQueue.RemoveAt (i);
			}

			for (int i = 0; i < mobSpawnQueue.Count; i++) {
				var mob = (GameObject)Instantiate (mobSpawnQueue [i].gameObj, mobSpawnQueue[i].position, Quaternion.identity);
				mob.SetActive (true);
				mobs.Add (new PlacedMob (mobSpawnQueue [i].name, mobSpawnQueue [i].position, mobSpawnQueue [i].id, mob));
				mobSpawnQueue.RemoveAt (i);
			}
		}

		public void AddPrefab (string objectId, int xPos, int zPos)
		{
			GameObject obj = prefabs.GetGameObject (objectId);
			if (obj == null) {
				return;
			}

			Vector3 position = new Vector3 (xPos, 0, zPos);

			roomSpawnQueue.Add(new Room (obj, position));
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

			mobSpawnQueue.Add (new Mob (obj, position, id, objectId));
		}

		public HashSet<PlacedPrefab> GetGameWorld()
		{
			return gameWorld;
		}

		public Vector3 GetVRPosition()
		{
			return vrPlayer.transform.position;
		}

		public HashSet<PlacedMob> GetMobs()
		{
			return mobs;
		}
	}

	struct Room
	{
		public GameObject gameObj;
		public Vector3 position;

		public Room(GameObject g, Vector3 p)
		{
			gameObj = g;
			position = p;
		}
	}

	struct Mob
	{
		public GameObject gameObj;
		public Vector3 position;
		public int id;
		public string name;

		public Mob(GameObject g, Vector3 p, int i, string n)
		{
			gameObj = g;
			position = p;
			id = i;
			name = n;
		}
	}
}