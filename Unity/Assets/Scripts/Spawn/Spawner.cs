using UnityEngine;
using System.Collections.Generic;

namespace Spawn
{
	public class Spawner : MonoBehaviour
	{
		public GameObject room1Prefab;

		private List<GameObject> spawnQueue;
		private List<Vector3> positions;

		void Start ()
		{
			spawnQueue = new List<GameObject> ();
			positions = new List<Vector3> ();
		}
	
		void Update ()
		{
			for (int i = 0; i < spawnQueue.Count; i++) {
				Instantiate (spawnQueue [i], positions [i], Quaternion.identity);
			}

			spawnQueue = new List<GameObject> ();
			positions = new List<Vector3> ();
		}

		public void addRoomPrefab (string objectId, int xPos, int zPos)
		{
			spawnQueue.Add (room1Prefab);
			positions.Add (new Vector3 (xPos, 0, zPos));
		}
	}
}