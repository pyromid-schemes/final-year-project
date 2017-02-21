﻿using System.Diagnostics;
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
            if (!_mob.IsAttacking())
            {
            }
            if (Vector3.Distance(_mob.Eyes.position, _player.transform.position) > _mob.AttackRange &&
                !_mob.IsAttacking())
            {
                _mob.ChangeState(SkeletonController.States.Pursue);
            }
            else
            {
                if (_timeSinceLastAttack > _mob.AttackCooldown)
                {
                    _mob.Attack();
                    _timeSinceLastAttack = 0f;
                }
                else
                {
                    _timeSinceLastAttack += Time.deltaTime;
                }
            }
        }

        public void OnFixedUpdate()
        {
            _mob.transform.LookAt(new Vector3(_player.transform.position.x, _mob.transform.position.y,
                _player.transform.position.z));
        }
    }
}