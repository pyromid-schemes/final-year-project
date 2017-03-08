using System;
using System.Collections.Generic;
using AI.MobControllers;
using AI.Pathfinding;
using UnityEngine;

namespace AI.States.Skeleton
{
    class PursueState : IState
    {
        private SkeletonController _mob;
        private CalculatePath _pathCalculator;
        private GameObject _player;

        public PursueState(SkeletonController mob)
        {
            _mob = mob;
        }

        public void OnEnter()
        {
            _mob.ToWalkingState();
            _player = GameObject.Find("Player");
        }

        public void OnUpdate()
        {
            if (!PlayerInSight())
            {
                _mob.ChangeState(SkeletonController.States.Patrol);
            }
            else if (Vector3.Distance(_mob.Eyes.position, _player.transform.position) < _mob.AttackRange)
            {
                _mob.ChangeState(SkeletonController.States.Defend);
            }
        }

        public void OnFixedUpdate()
        {
            _pathCalculator = new CalculatePath(_mob.Grid.GetGrid());
            List<PathfindingNode> path = _pathCalculator.GetPathToDestination(_mob.transform.position.x,
                _mob.transform.position.z, _player.transform.position.x, _player.transform.position.z);
            PathfindingNode nextNode = path[0];
            _mob.transform.position = Vector3.MoveTowards(_mob.transform.position,
                new Vector3(nextNode.X, _mob.transform.position.y, nextNode.Z), 0.5f*Time.deltaTime);
            _mob.transform.LookAt(new Vector3(_player.transform.position.x, _mob.transform.position.y,
                _player.transform.position.z));
        }

        private bool PlayerInSight()
        {
            if (Physics.CheckSphere(_mob.transform.position, 20f, LayerMask.GetMask("Player")))
            {
                Vector3 forward = _mob.transform.forward;
                Vector3 _playerPos = _player.transform.position;
                if (Vector3.Dot(Vector3.Normalize(forward), Vector3.Normalize(_playerPos)) >= 0)
                {
                    RaycastHit hit;
                    Vector3 direction = _mob.transform.position - _playerPos;
                    Debug.Log(direction);
                    if (!Physics.Raycast(_mob.transform.position, direction, out hit, LayerMask.GetMask("World Geometry")))
                        return true;
                }
            }
            return false;
        }
    }
}