using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;

namespace AI.Pathfinding
{
    public class Grid : MonoBehaviour
    {
        private GridManager _gridManager;
        public static float _spaceBetween = 0.5f;
        public bool DebugMode = false;
        public DebugSphere testThing;

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
                    _gridManager.AddNode(child.transform.position.x, child.transform.position.z);

                    if (DebugMode)
                    {
                        DebugSphere debug = (DebugSphere) Instantiate(
                                testThing,
                                new Vector3(child.transform.position.x, 2.5f, child.transform.position.z),
                                child.transform.rotation);
                        debug.parent = child.gameObject;
                    }
                }
            }
        }

        public GridManager GetGrid()
        {
            return _gridManager;
        }
    }
}