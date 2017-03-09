using AI.States;
using AI.States.Skeleton;
using UnityEngine;

namespace AI.MobControllers
{
    class SkeletonController : SimpleMobController
    {
        [HideInInspector]
        public enum States
        {
            Patrol,
            Pursue,
            Defend
        }

        protected override void InitialiseStates()
        {
            StateManager = new StateManager();

            // Add states to manager
            StateManager.AddState((int)States.Patrol, new PatrolState(this));
            StateManager.AddState((int)States.Pursue, new PursueState(this));
            StateManager.AddState((int)States.Defend, new DefendState(this));

            // Set starting state
            StateManager.SetCurrentState((int)States.Patrol);
            StateManager.GetCurrentState().OnEnter();

            // Setup transitions
            CreateStateTransitions();
        }

        private void CreateStateTransitions()
        {
            StateManager.AddTransition((int)States.Patrol, (int)States.Pursue);

            StateManager.AddTransition((int)States.Pursue, (int)States.Patrol);
            StateManager.AddTransition((int)States.Pursue, (int)States.Defend);

            StateManager.AddTransition((int)States.Defend, (int)States.Pursue);

        }

        public void ChangeState(States nextState)
        {
            StateManager.ChangeToState((int)nextState);
        }

        // I think this could be done cleaner @DanC
        // Idle = default
        // Walking -> Idle
        public void ToIdleState()
        {
            SetWalking(false);
            SetIdle(true);
        }

        // Idle -> Walking
        // Defending -> Walking
        public void ToWalkingState()
        {
            SetDefend(false);
            SetIdle(false);
            SetWalking(true);
        }

        //Walking -> Defend
        //Attacking -> Defend
        public void ToDefendingState()
        {
            SetWalking(false);
            SetDefend(true);
        }

        public bool IsAttacking()
        {
            return Anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack");
        }

        void SetWalking(bool isWalking)
        {
            Anim.SetBool("walking", isWalking);
        }

        void SetIdle(bool isIdle)
        {
        }

        public void Attack()
        {
            Anim.SetTrigger("attack");
        }

        void SetDefend(bool isDefend)
        {
            Anim.SetBool("defending", isDefend);
        }

        void TakeDamage()
        {
            Anim.SetTrigger("damageTaken");
        }

        void Die()
        {
            Anim.SetTrigger("death");
        }
    }
}