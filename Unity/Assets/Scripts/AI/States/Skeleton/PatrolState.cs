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
            if(Physics.CheckSphere(_mob.transform.position, 5f, LayerMask.GetMask("Player")))
            {
                Vector3 forward = _mob.transform.forward;
                Vector3 _playerPos = _player.transform.position;
                if (Vector3.Dot(Vector3.Normalize(forward), Vector3.Normalize(_playerPos)) >=0)
                {
                    RaycastHit hit;
                    Vector3 direction = _mob.transform.position - _playerPos;
                    Debug.Log(direction);
                    if(!Physics.Raycast(_mob.transform.position, direction, out hit, LayerMask.GetMask("World Geometry")))
                        _mob.ChangeState(SkeletonController.States.Pursue);
                }
            }
        }

        public void OnFixedUpdate()
        {
            _mob.transform.Rotate(Vector3.up, Time.deltaTime*_mob.RotateSpeed);
        }
    }
}