using AI.MobControllers;
using UnityEngine;

namespace AI.States.Skeleton
{
    class PatrolState : IState
    {
        private readonly SkeletonController _mob;
        private readonly GameObject _player;


        public PatrolState(SkeletonController mob)
        {
            _mob = mob;
            _player = GameObject.Find("Player");
        }

        public void OnEnter()
        {
            _mob.ToIdleState();
        }

        public void OnUpdate()
        {
            if (!Physics.CheckSphere(_mob.transform.position, _mob.MaxSightRange, LayerMask.GetMask("Player"))) return;
//            var forward = _mob.transform.forward;
//            var playerPos = _player.transform.position;
//            if (!(Vector3.Dot(Vector3.Normalize(forward), Vector3.Normalize(playerPos)) >= 0)) return;
//            RaycastHit hit;
//            var direction = Vector3.Normalize(playerPos - _mob.transform.position);
//            if (!Physics.Raycast(_mob.transform.position, direction, out hit) ||
//                !hit.collider.CompareTag("Player")) return;
            _mob.ChangeState(SkeletonController.States.Pursue);
        }

        public void OnFixedUpdate()
        {
            _mob.transform.Rotate(Vector3.up, Time.deltaTime * _mob.RotateSpeed);
        }
    }
}