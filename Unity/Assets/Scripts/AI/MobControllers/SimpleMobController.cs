using AI.Pathfinding;
using AI.States;
using UnityEngine;

namespace AI.MobControllers
{
    abstract class SimpleMobController : MonoBehaviour
    {
        protected StateManager StateManager;
        protected Animator Anim;
        [HideInInspector] public Grid Grid;

        public Transform Eyes;
        public float AttackCooldown = 2f;
        public float AttackRange = 2f;
        public float RotateSpeed = 10f;
        public float FOV = 180f;

        protected abstract void InitialiseStates();

        void Start()
        {
            Anim = GetComponent<Animator>();
            Grid = GameObject.Find("Grid").GetComponent<Grid>();
            InitialiseStates();
        }

        // Update is called once per frame
        void Update()
        {
            StateManager.GetCurrentState().OnUpdate();
        }

        void FixedUpdate()
        {
            StateManager.GetCurrentState().OnFixedUpdate();
        }
    }
}