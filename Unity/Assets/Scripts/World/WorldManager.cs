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

		private List<PlacedPrefab> gameWorld;
		private List<PlacedMob> mobs;

		void Start ()
		{
			roomSpawnQueue = new List<Room> ();
			mobSpawnQueue = new List<Mob> ();
			gameWorld = new List<PlacedPrefab> ();
			mobs = new List<PlacedMob> ();
			AddPrefab ("room2", 0, 0, 0);
		}
	
		void Update ()
		{
			for (int i = 0; i < roomSpawnQueue.Count; i++) {
				var obj = (GameObject)Instantiate (roomSpawnQueue [i].gameObj, roomSpawnQueue[i].position, roomSpawnQueue [i].rotation);
				obj.SetActive (true);
				roomSpawnQueue.RemoveAt (i);
			}

			mobs.RemoveAll (mob => mob.HasBeenKilled ());

			for (int i = 0; i < mobSpawnQueue.Count; i++) {
				var mob = (GameObject)Instantiate (mobSpawnQueue [i].gameObj, mobSpawnQueue[i].position, Quaternion.identity);
				mob.SetActive (true);
				mobs.Add (new PlacedMob (mobSpawnQueue [i].name, mobSpawnQueue [i].position, mobSpawnQueue [i].id, mob));
				mobSpawnQueue.RemoveAt (i);
			}
		}

		public void AddPrefab (string objectId, int xPos, int zPos, int rot)
		{
			GameObject obj = prefabs.GetGameObject (objectId);
			if (obj == null) {
				return;
			}

			Vector3 position = new Vector3 (xPos, 0, zPos);
			Quaternion rotation = Quaternion.Euler (0, rot, 0);

			roomSpawnQueue.Add(new Room (obj, position, rotation));
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

		public List<PlacedPrefab> GetGameWorld()
		{
			return gameWorld;
		}

		public Vector3 GetVRPosition()
		{
			return vrPlayer.transform.position;
		}

		public List<PlacedMob> GetMobs()
		{
			return mobs;
		}
	}

	struct Room
	{
		public GameObject gameObj;
		public Vector3 position;
		public Quaternion rotation;

		public Room(GameObject g, Vector3 p, Quaternion r)
		{
			gameObj = g;
			position = p;
			rotation = r;
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