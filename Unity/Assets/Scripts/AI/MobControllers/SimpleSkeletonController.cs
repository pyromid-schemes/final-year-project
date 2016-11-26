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
            Attack
        }

        [HideInInspector] public NavMeshAgent Agent;

        public float RotateSpeed = 10f;

        void Start()
        {
            Agent = GetComponent<NavMeshAgent>();

            _stateManager = new StateManager();

            _stateManager.AddState((int) States.Patrol, new PatrolState(this));
            _stateManager.AddState((int) States.Pursue, new PursueState(this));

            _stateManager.SetCurrentState((int) States.Patrol);
            _stateManager.AddTransition((int) States.Patrol, (int) States.Pursue);
        }

        public void ChangeState(States nextState)
        {
            _stateManager.ChangeToState((int) nextState);
        }
    }
}