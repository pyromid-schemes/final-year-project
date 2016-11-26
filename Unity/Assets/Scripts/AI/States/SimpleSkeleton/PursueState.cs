using AI.MobControllers;
using UnityEngine;

namespace AI.States.SimpleSkeleton
{
    class PursueState : IState
    {

        private SimpleSkeletonController _mob;

        public PursueState(SimpleSkeletonController mob)
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
            _mob.Agent.destination = GameObject.Find("Player").transform.position;
        }
    }
}
