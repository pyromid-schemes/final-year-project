using AI.MobControllers;
using UnityEngine;

namespace AI.States.SimpleSkeleton
{
    class PatrolState : IState
    {
        private readonly SimpleSkeletonController _mob;

        public PatrolState(SimpleSkeletonController mob)
        {
            _mob = mob;
        }

        public void OnEnter()
        {
        }

        public void OnUpdate()
        {
            RaycastHit hit;
            for (float i = -90f; i < 90f; i++)
            {
                Vector3 direction = _mob.transform.forward - _mob.transform.right*i;
                if (Physics.Raycast(_mob.transform.position, direction, out hit) && hit.collider.CompareTag("Player"))
                {
                    _mob.ChangeState(SimpleSkeletonController.States.Pursue);
                }
            }
        }

        public void OnFixedUpdate()
        {
//            _mob.transform.Rotate(Vector3.up, Time.deltaTime*_mob.RotateSpeed);
        }
    }
}