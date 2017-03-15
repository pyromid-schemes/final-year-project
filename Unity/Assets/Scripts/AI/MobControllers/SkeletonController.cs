using System.Collections.Generic;
using AI.Pathfinding;
using AI.States;
using AI.States.Skeleton;
using UnityEngine;

namespace AI.MobControllers
{
    class SkeletonController : SimpleMobController
    {
        private States _currentStateId;

        [HideInInspector]
        public enum States
        {
            Patrol,
            Pursue,
            Defend
        }

        public override HashSet<PathfindingNode> GetOccupiedSpaces()
        {
            PathfindingNode curNode;
            if (IsState(States.Pursue) && ((PursueState) StateManager.GetCurrentState()).GetPath().Count > 0)
            {
                var path = ((PursueState) StateManager.GetCurrentState()).GetPath();
                curNode = path[0];
            }
            else
            {
                var closest = Grid.GetGrid().GetClosestNode(transform.position.x, transform.position.z);
                curNode = new PathfindingNode(closest.X, closest.Z);
            }
            var spaceBetween = Grid.SpaceBetween;
            var curNodeX = curNode.X;
            var curNodeZ = curNode.Z;
            return new HashSet<PathfindingNode>
            {
                new PathfindingNode(curNodeX - spaceBetween, curNodeZ),
                new PathfindingNode(curNodeX + spaceBetween, curNodeZ),
                new PathfindingNode(curNodeX, curNodeZ - spaceBetween),
                new PathfindingNode(curNodeX, curNodeZ + spaceBetween),
                new PathfindingNode(curNodeX - spaceBetween, curNodeZ + spaceBetween),
                new PathfindingNode(curNodeX + spaceBetween, curNodeZ + spaceBetween),
                new PathfindingNode(curNodeX - spaceBetween, curNodeZ - spaceBetween),
                new PathfindingNode(curNodeX + spaceBetween, curNodeZ - spaceBetween)
            };
        }

        protected override void InitialiseStates()
        {
            StateManager = new StateManager();

            // Add states to manager
            StateManager.AddState((int) States.Patrol, new PatrolState(this));
            StateManager.AddState((int) States.Pursue, new PursueState(this));
            StateManager.AddState((int) States.Defend, new DefendState(this));

            // Set starting state
            StateManager.SetCurrentState((int) States.Patrol);
            _currentStateId = States.Patrol;
            StateManager.GetCurrentState().OnEnter();

            // Setup transitions
            CreateStateTransitions();
        }

        private void CreateStateTransitions()
        {
            StateManager.AddTransition((int) States.Patrol, (int) States.Pursue);

            StateManager.AddTransition((int) States.Pursue, (int) States.Patrol);
            StateManager.AddTransition((int) States.Pursue, (int) States.Defend);

            StateManager.AddTransition((int) States.Defend, (int) States.Pursue);
        }

        public void ChangeState(States nextState)
        {
            StateManager.ChangeToState((int) nextState);
            _currentStateId = nextState;
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


        public bool IsState(States state)
        {
            return _currentStateId.Equals(state);
        }

        public bool IsAttacking()
        {
            return Anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack");
        }

        public bool IsDying()
        {
            return Anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Death");
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