using AI.MobControllers;
using AI.Pathfinding;
using UnityEngine;

namespace AI.States.Skeleton
{
    internal class PursueState : IState
    {
        private readonly SkeletonController _mob;
        private CalculatePath _pathCalculator;
        private GameObject _player;
        private bool _playerInSight;
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
            else if (Vector3.Distance(_mob.transform.position, _player.transform.position) < _mob.AttackRange)
            {
                _mob.ChangeState(SkeletonController.States.Defend);
            }
        }

        public void OnFixedUpdate()
        {
            _pathCalculator = new CalculatePath(_mob.Grid.GetGrid());
            var path = _pathCalculator.GetPathToDestination(_mob.transform.position.x,
                _mob.transform.position.z, _player.transform.position.x, _player.transform.position.z);
            var nextNode = path[0];
            _mob.transform.position = Vector3.MoveTowards(_mob.transform.position,
                new Vector3(nextNode.X, _mob.transform.position.y, nextNode.Z), 0.5f * Time.deltaTime);
            _mob.transform.LookAt(new Vector3(_player.transform.position.x, _mob.transform.position.y,
                _player.transform.position.z));
            _playerInSight = PlayerInSight();
        }

        private bool PlayerInSight()
        {
            var playerPos = _player.transform.position;
            RaycastHit hit;
            var direction = Vector3.Normalize(playerPos - _mob.transform.position);
            var selfPos = _mob.transform.position;
            selfPos.y += 1f;
            Debug.DrawRay(selfPos, direction, Color.green, _mob.MaxSightRange);
            var ret = Physics.Raycast(_mob.transform.position, direction, out hit, _mob.MaxSightRange) && hit.collider.CompareTag("Player");
            return ret;
        }
    }
}