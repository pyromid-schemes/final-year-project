using System.Diagnostics;
using System.Runtime.InteropServices;
using AI.MobControllers;
using UnityEngine;
using Valve.VR;
using Debug = UnityEngine.Debug;

namespace AI.States.SimpleSkeleton
{
    class DefendState : IState
    {

        private SimpleSkeletonController _mob;
        private GameObject _player;

        private float _timeSinceLastAttack = 0f;

        public DefendState(SimpleSkeletonController mob)
        {
            _mob = mob;
        }

        public void OnEnter()
        {
            _mob.ToDefendingState();
            _player = GameObject.Find("Player");
            _timeSinceLastAttack = 0f;
            _mob.Agent.Stop();
        }

        public void OnUpdate()
        {
            if (Vector3.Distance(_mob.Eyes.position, _player.transform.position) > _mob.AttackRange)
            {
                _mob.ChangeState(SimpleSkeletonController.States.Pursue);
            }
            else
            {
                if (_timeSinceLastAttack > _mob.AttackCooldown)
                {
                    _mob.ChangeState(SimpleSkeletonController.States.Attack);
                }
                else
                {
                    _timeSinceLastAttack += Time.deltaTime;
                }
            }
        }

        public void OnFixedUpdate()
        {
        }
    }
}
