using System.Collections;
using AI.MobControllers;
using UnityEngine;

namespace AI.States.Skeleton
{
    class DefendState : IState
    {
        private readonly SkeletonController _mob;
        private GameObject _player;
        private Coroutine _attack;

        public DefendState(SkeletonController mob)
        {
            _mob = mob;
        }

        public void OnEnter()
        {
            _mob.ToDefendingState();
            _attack = _mob.StartCoroutine(Attack());
        }

        public void OnUpdate()
        {
            _player = _mob.WorldManager.GetVRPlayer();
            var playerCheck = new Vector3(_player.transform.position.x, _mob.transform.position.y,
                _player.transform.position.z);
            if (Vector3.Distance(_mob.transform.position, playerCheck) > _mob.AttackRange + 1f &&
                !_mob.IsAttacking())
            {
                _mob.StopCoroutine(_attack);
                _mob.ChangeState(SkeletonController.States.Pursue);
            }
            else if (_mob.IsDying())
            {
                _mob.StopCoroutine(_attack);
            }
        }

        public void OnFixedUpdate()
        {
            _mob.transform.LookAt(new Vector3(_player.transform.position.x, _mob.transform.position.y,
                _player.transform.position.z));
        }

        private IEnumerator Attack()
        {
            for (;;)
            {
                _mob.Sword.setWeaponIsActive(true);
                _mob.Attack();
                while (_mob.IsAttacking())
                {
                }
                _mob.Sword.setWeaponIsActive(false);
                yield return new WaitForSeconds(_mob.AttackCooldown);
            }
        }
    }
}