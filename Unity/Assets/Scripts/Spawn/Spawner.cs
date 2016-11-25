using UnityEngine;
using System.Collections.Generic;

namespace Spawn
{
	public class Spawner : MonoBehaviour, ISpawner
	{
		public PrefabMap prefabs;

		private bool queueActive;
		private List<PositionalGameObject> objectsToBeBuilt;

		void Start ()
		{
			objectsToBeBuilt = new List<PositionalGameObject> ();
			queueActive = false;
		}
	
		void Update ()
		{
			if (queueActive) {
				for (int i = 0; i < objectsToBeBuilt.Count; i++) {
					var obj = (GameObject)Instantiate (objectsToBeBuilt [i].gameObj, objectsToBeBuilt[i].position, Quaternion.identity);
					obj.SetActive (true);
					objectsToBeBuilt.RemoveAt (i);
				}
				queueActive = false;
			}
		}

		public void AddRoomPrefab (string objectId, int xPos, int zPos)
		{
			GameObject room = prefabs.GetGameObject (objectId);
			if (room == null) {
				return;
			}

			queueActive = true;
			objectsToBeBuilt.Add(new PositionalGameObject (room, new Vector3 (xPos, 0, zPos)));
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