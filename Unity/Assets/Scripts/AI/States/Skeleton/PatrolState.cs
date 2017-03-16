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
        }

        public void OnEnter()
        {
            _mob.ToIdleState();
        }

        public void OnUpdate()
        {
            if (!Physics.CheckSphere(_mob.transform.position, _mob.MaxSightRange, LayerMask.GetMask("Player"))) return;
            var playerPos = _mob.WorldManager.GetVRPlayer().transform.position;
            var playerCheck = new Vector3(playerPos.x, _mob.transform.position.y, playerPos.z);
            var direction = Vector3.Normalize(playerCheck - _mob.transform.position);
            var forward = _mob.transform.forward;
            var dot = Vector3.Dot(Vector3.Normalize(direction), Vector3.Normalize(forward));
            if (!(dot >= 0)) return;
            RaycastHit hit;
            var selfPos = _mob.transform.position;
            selfPos.y += 1f;
            if (!Physics.Raycast(selfPos, direction, out hit) ||
                !hit.collider.CompareTag("Player")) return;
            _mob.ChangeState(SkeletonController.States.Pursue);
        }

        public void OnFixedUpdate()
        {
            _mob.transform.Rotate(Vector3.up, Time.deltaTime * _mob.RotateSpeed);
        }
    }
}