using System.Collections.Generic;
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
        [HideInInspector] public Sword Sword;
        public Transform Eyes;
        public float AttackCooldown = 2f;
        public float AttackRange = 2f;
        public float MaxSightRange = 10f;
        public float RotateSpeed = 10f;
        public float Fov = 180f; // Not in use for now i don't think ?

        public abstract List<PathfindingNode> GetOccupiedSpaces();

        protected abstract void InitialiseStates();

        void Start()
        {
            Anim = GetComponent<Animator>();
            Grid = GameObject.Find("Grid").GetComponent<Grid>();
            Sword = GetComponentInChildren<Sword>();
            Sword.setWeaponIsActive(false);
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