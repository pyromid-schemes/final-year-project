using System;
using AI.MobControllers;
using UnityEngine;

namespace AI.States.SimpleSkeleton
{
    class PursueState : IState
    {

        private SimpleSkeletonController _mob;

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
            if (Vector3.Distance(_mob.Eyes.position, _player.transform.position) < _mob.AttackRange)
            {
                _mob.ChangeState(SimpleSkeletonController.States.Defend);
            }
        }

        public void OnFixedUpdate()
        {
            _mob.Agent.Resume();
            _mob.Agent.destination = _player.transform.position;
        }
    }
}
