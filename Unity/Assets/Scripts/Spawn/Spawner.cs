﻿using UnityEngine;
using System.Collections.Generic;

namespace Spawn
{
	public class Spawner : MonoBehaviour, ISpawner
	{
		public PrefabMap prefabs;

		private bool queueActive;
		private List<PositionalGameObject> spawnQueue;
		private HashSet<PlacedPrefab> gameWorld;

		void Start ()
		{
			spawnQueue = new List<PositionalGameObject> ();
			gameWorld = new HashSet<PlacedPrefab> ();
			queueActive = false;
			AddPrefab ("room1", 0, 0);
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
			gameWorld.Add (new PlacedPrefab (objectId, position));
		}

		public HashSet<PlacedPrefab> GetGameWorld()
		{
			return gameWorld;
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