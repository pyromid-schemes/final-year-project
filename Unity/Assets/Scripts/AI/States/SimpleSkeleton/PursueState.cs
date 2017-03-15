using System;
using AI.MobControllers;
using AI.Pathfinding;
using UnityEngine;

namespace AI.States.SimpleSkeleton
{
    class PursueState : IState
    {
        private readonly SimpleSkeletonController _mob;
        private CalculatePath _pathCalculator;
        private GameObject _player;

        public PursueState(SimpleSkeletonController mob)
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
                _mob.ChangeState(SimpleSkeletonController.States.Patrol);
            }
            else if (Vector3.Distance(_mob.Eyes.position, _player.transform.position) < _mob.AttackRange)
            {
                _mob.ChangeState(SimpleSkeletonController.States.Defend);
            }
        }

        public void OnFixedUpdate()
        {
            _pathCalculator = new CalculatePath(_mob.Grid.GetGrid(), _mob.gameObject.GetInstanceID());
            var path = _pathCalculator.GetPathToDestination(_mob.transform.position.x,
                _mob.transform.position.z, _player.transform.position.x, _player.transform.position.z);
            PathfindingNode nextNode;
            if (
                Math.Abs(_mob.transform.position.x - path[0].X) > .5f &&
                Math.Abs(_mob.transform.position.z - path[0].Z) > .5f ||
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
                new Vector3(nextNode.X, _mob.transform.position.y, nextNode.Z), 0.5f * Time.deltaTime);
            _mob.transform.LookAt(new Vector3(_player.transform.position.x, _mob.transform.position.y,
                _player.transform.position.z));
        }

        private bool PlayerInSight()
        {
            for (var i = -90f; i < 90f; i++)
            {
                var direction = _mob.Eyes.forward - _mob.Eyes.right * i;
                RaycastHit hit;
                if (Physics.Raycast(_mob.Eyes.position, direction, out hit) && hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}