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

		private bool queueActive;
		private List<PositionalGameObject> spawnQueue;
		private HashSet<PlacedPrefab> gameWorld;

		void Start ()
		{
			spawnQueue = new List<PositionalGameObject> ();
			gameWorld = new HashSet<PlacedPrefab> ();
			queueActive = false;
			AddPrefab ("room2", 0, 0);
		}
	
		void Update ()
		{
			if (queueActive) {
				for (int i = 0; i < spawnQueue.Count; i++) {
					var obj = (GameObject)Instantiate (spawnQueue [i].gameObj, spawnQueue[i].position, Quaternion.identity);
					obj.SetActive (true);
					spawnQueue.RemoveAt (i);
				}
				queueActive = false;
			}
		}

		public void AddPrefab (string objectId, int xPos, int zPos)
		{
			// TODO change where the name of the room is resolved
			GameObject obj = prefabs.GetGameObject (objectId);
			if (obj == null) {
				return;
			}

			queueActive = true;
			Vector3 position = new Vector3 (xPos, 0, zPos);

			spawnQueue.Add(new PositionalGameObject (obj, position));
		    grid.AddNodes(obj);
			gameWorld.Add (new PlacedPrefab (objectId, position));
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
}