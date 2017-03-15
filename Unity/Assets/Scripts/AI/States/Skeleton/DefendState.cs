using AI.MobControllers;
using UnityEngine;

namespace AI.States.Skeleton
{
    class DefendState : IState
    {
        private readonly SkeletonController _mob;
        private GameObject _player;

        private float _timeSinceLastAttack;

        public DefendState(SkeletonController mob)
        {
            _mob = mob;
        }

        public void OnEnter()
        {
            _mob.ToDefendingState();
            _timeSinceLastAttack = 0f;
            _player = _mob.WorldManager.GetVRPlayer();
        }

        public void OnUpdate()
        {
            var playerCheck = new Vector3(_player.transform.position.x, _mob.transform.position.y,
                _player.transform.position.z);
            if (Vector3.Distance(_mob.transform.position, playerCheck) > _mob.AttackRange &&
                !_mob.IsAttacking())
            {
                _mob.Sword.setWeaponIsActive(false);
                _mob.ChangeState(SkeletonController.States.Pursue);
            }
            else
            {
                if (_timeSinceLastAttack > _mob.AttackCooldown)
                {
                    _mob.Sword.setWeaponIsActive(true);
                    _mob.Attack();
                    _timeSinceLastAttack = 0f;
                }
                else
                {
                    _mob.Sword.setWeaponIsActive(false);
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