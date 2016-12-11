using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace AI.Pathfinding
{
    public class Grid : MonoBehaviour
    {
        private GridManager _gridManager;
        public static float _spaceBetween = 1f;

        void Awake()
        {
            _gridManager = new GridManager();
        }

        public void AddNodes(GameObject room)
        {
            Component[] children = room.GetComponentsInChildren(typeof(Transform));
            foreach (Component child in children)
            {
                if (child.tag == "Walkable")
                {
                    float x = child.transform.position.x;
                    float z = child.transform.position.z;
                    _gridManager.AddNode(x, z);
                }
            }
        }

        public GridManager GetGrid()
        {
            return _gridManager;
        }
    }
}