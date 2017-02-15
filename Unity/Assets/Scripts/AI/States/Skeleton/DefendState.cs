using System.Diagnostics;
using System.Runtime.InteropServices;
using AI.MobControllers;
using UnityEngine;
using Valve.VR;
using Debug = UnityEngine.Debug;

namespace AI.States.Skeleton
{
    class DefendState : IState
    {

        private SkeletonController _mob;
        private GameObject _player;

        private float _timeSinceLastAttack = 0f;

        public DefendState(SkeletonController mob)
        {
            _mob = mob;
        }

        public void OnEnter()
        {
            _mob.ToDefendingState();
            _player = GameObject.Find("Player");
            _timeSinceLastAttack = 0f;
        }

        public void OnUpdate()
        {
            if (Vector3.Distance(_mob.Eyes.position, _player.transform.position) > _mob.AttackRange)
            {
                _mob.ChangeState(SkeletonController.States.Pursue);
            }
            else
            {
                if (_timeSinceLastAttack > _mob.AttackCooldown)
                {
                    _mob.ChangeState(SkeletonController.States.Attack);
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
