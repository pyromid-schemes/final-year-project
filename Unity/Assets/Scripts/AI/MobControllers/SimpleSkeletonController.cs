using System;
using System.Collections.Generic;
using AI.Pathfinding;
using AI.States;
using AI.States.SimpleSkeleton;
using UnityEngine;

namespace AI.MobControllers
{
    class SimpleSkeletonController : SimpleMobController
    {
        [HideInInspector]
        public enum States
        {
            Patrol,
            Pursue,
            Defend,
            Attack
        }

        [HideInInspector] public Grid Grid;
        public Transform Eyes;
        public float AttackCooldown = 2f;
        public float AttackRange = 2f;
        public float RotateSpeed = 10f;
        public float FOV = 180f;

        private void InitialiseStates()
        {
            StateManager = new StateManager();

            // Add states to manager
            StateManager.AddState((int) States.Patrol, new PatrolState(this));
            StateManager.AddState((int) States.Pursue, new PursueState(this));
            StateManager.AddState((int) States.Defend, new DefendState(this));
            StateManager.AddState((int) States.Attack, new AttackState(this));

            // Set starting state
            StateManager.SetCurrentState((int)States.Patrol);

            // Setup transitions
            CreateStateTransitions();
        }

        private void CreateStateTransitions()
        {
            StateManager.AddTransition((int) States.Patrol, (int) States.Pursue);

            StateManager.AddTransition((int) States.Pursue, (int) States.Patrol);
            StateManager.AddTransition((int) States.Pursue, (int) States.Defend);

            StateManager.AddTransition((int) States.Defend, (int) States.Attack);
            StateManager.AddTransition((int) States.Defend, (int) States.Pursue);

            StateManager.AddTransition((int) States.Attack, (int) States.Defend);
        }

        void Start()
        {
            Anim = GetComponent<Animator>();
            Grid = GameObject.Find("Grid").GetComponent<Grid>();
            InitialiseStates();
        }

        public void ChangeState(States nextState)
        {
            StateManager.ChangeToState((int) nextState);
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
            SetAttack(false);
            SetDefend(true);
        }

        // Defending -> Attack
        public void ToAttackingState()
        {
            SetDefend(false);
            SetAttack(true);
        }

        public bool IsAttacking()
        {
            return Anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack");
        }

        void SetWalking(bool isWalking)
        {
            Anim.SetBool("isWalking", isWalking);
        }

        void SetIdle(bool isIdle)
        {
            Anim.SetBool("isIdling", isIdle);
        }

        void SetAttack(bool isAttack)
        {
            Anim.SetBool("isAttacking", isAttack);
        }

        void SetDefend(bool isDefend)
        {
            Anim.SetBool("isDefending", isDefend);
        }
    }
}