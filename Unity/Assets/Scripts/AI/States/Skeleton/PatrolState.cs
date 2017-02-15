using AI.MobControllers;
using UnityEngine;

namespace AI.States.Skeleton
{
    class PatrolState : IState
    {
        private readonly SkeletonController _mob;

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
            RaycastHit hit;
            for (float i = -(_mob.FOV/2); i < (_mob.FOV/2); i++)
            {
                Vector3 direction = _mob.Eyes.forward - _mob.Eyes.right*i;
                if (Physics.Raycast(_mob.Eyes.position, direction, out hit) && hit.collider.CompareTag("Player"))
                {
                    _mob.ChangeState(SkeletonController.States.Pursue);
                    break;
                }
            }
        }

        public void OnFixedUpdate()
        {
            _mob.transform.Rotate(Vector3.up, Time.deltaTime*_mob.RotateSpeed);
        }
    }
}