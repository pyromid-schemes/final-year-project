using System;
using System.Collections.Generic;
using AI.MobControllers;
using UnityEngine;
using World;

namespace AI.Pathfinding
{
    public class Grid : MonoBehaviour, IGrid
    {
        private GridManager _gridManager;
        public WorldManager WorldManager;
        public static float SpaceBetween = 0.5f;
        public bool DebugMode;
        public DebugSphere TestThing;
        private Dictionary<int, HashSet<PathfindingNode>> _mobPositions;

        public void Awake()
        {
            _gridManager = new GridManager(this);
            _mobPositions = new Dictionary<int, HashSet<PathfindingNode>>();
        }

        public void FixedUpdate()
        {
            _mobPositions.Clear();
            var mobs = WorldManager.GetMobs();
            foreach (var mob in mobs)
            {
                var controller = mob.GetGameObject().GetComponent<SimpleMobController>();
                var spaces = controller.GetOccupiedSpaces();
                _mobPositions.Add(mob.GetGameObject().name.GetHashCode(), spaces);
            }
        }

        public void AddNodes(GameObject room)
        {
            var children = room.GetComponentsInChildren(typeof(Transform));
            foreach (var child in children)
            {
                if (!child.CompareTag("Walkable")) continue;
                double rounding = SpaceBetween / 2;
                var x = Math.Round(child.transform.position.x / rounding) * rounding;
                var z = Math.Round(child.transform.position.z / rounding) * rounding;
                _gridManager.AddNode((float) x, (float) z);

                if (!DebugMode) continue;
                var debug = (DebugSphere) Instantiate(
                    TestThing,
                    new Vector3(child.transform.position.x, 2.5f, child.transform.position.z),
                    child.transform.rotation);
                debug.parent = child.gameObject;
            }
        }

        public GridManager GetGrid()
        {
            return _gridManager;
        }

        public Dictionary<int, HashSet<PathfindingNode>> GetMobPositions()
        {
            return _mobPositions;
        }
    }
}