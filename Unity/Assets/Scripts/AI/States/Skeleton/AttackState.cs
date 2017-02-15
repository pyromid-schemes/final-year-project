using AI.MobControllers;

namespace AI.States.Skeleton
{
    class AttackState : IState
    {

        private SkeletonController _mob;

        public AttackState(SkeletonController mob)
        {
            _mob = mob;
        }

        public void OnEnter()
        {
            _mob.ToAttackingState();
        }

        public void OnUpdate()
        {
            if (!_mob.IsAttacking())
            {
                _mob.ChangeState(SkeletonController.States.Defend);
            }
        }

        public void OnFixedUpdate()
        {
        }
    }
}
