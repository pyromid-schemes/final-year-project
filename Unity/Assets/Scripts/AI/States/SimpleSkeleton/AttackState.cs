using AI.MobControllers;

namespace AI.States.SimpleSkeleton
{
    class AttackState : IState
    {

        private SimpleSkeletonController _mob;

        public AttackState(SimpleSkeletonController mob)
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
                _mob.ChangeState(SimpleSkeletonController.States.Defend);
            }
        }

        public void OnFixedUpdate()
        {
        }
    }
}
