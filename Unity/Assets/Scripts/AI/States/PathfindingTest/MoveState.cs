using System;
using System.Collections.Generic;
using AI.MobControllers;
using AI.Pathfinding;
using UnityEngine;

namespace AI.States.PathfindingTest
{
    class MoveState : IState
    {
        private PathfindingTestController _mob;
        private CalculatePath _pathCalculator;

        public MoveState(PathfindingTestController mob)
        {
            _mob = mob;
        }

        public void OnEnter()
        {
        }

        public void OnUpdate()
        {
        }

        public void OnFixedUpdate()
        {
            _pathCalculator = new CalculatePath(_mob.Grid.GetGrid());
            Transform player = GameObject.Find("MockVRPlayer").transform;
            List<PathfindingNode> path = _pathCalculator.GetPathToDestination(_mob.transform.position.x,
                _mob.transform.position.z, player.position.x, player.position.z);
            PathfindingNode nextNode;
            if (
                (Math.Abs(_mob.transform.position.x - path[0].X) > .5f &&
                 Math.Abs(_mob.transform.position.z - path[0].Z) > .5f) ||
                path.Count == 1
                )
            {
                nextNode = path[0];
            }
            else
            {
                nextNode = path[1];
            }
            _mob.transform.position = Vector3.MoveTowards(_mob.transform.position,
                new Vector3(nextNode.X, _mob.transform.position.y, nextNode.Z), 0.5f*Time.deltaTime);
        }
    }
}