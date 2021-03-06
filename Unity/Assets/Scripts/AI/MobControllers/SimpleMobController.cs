﻿using System.Collections.Generic;
using AI.Pathfinding;
using AI.States;
using UnityEngine;
using World;

/*
 * @author Daniel Burnley
 */
namespace AI.MobControllers
{
    abstract class SimpleMobController : MonoBehaviour
    {
        protected StateManager StateManager;
        protected Animator Anim;
        [HideInInspector] public Grid Grid;
        [HideInInspector] public WorldManager WorldManager;
        [HideInInspector] public Sword Sword;
        [HideInInspector] public SkeletonHealth Health;
        public Transform Eyes;
        public float AttackCooldown = 2f;
        public float AttackRange = 2f;
        public float AggroRange = 2f;
        public float MaxSightRange = 10f;
        public float RotateSpeed = 10f;
        public float Speed = 1f;
        public float Fov = 180f; // Not in use for now i don't think ?

        public abstract HashSet<PathfindingNode> GetOccupiedSpaces();

        protected abstract void InitialiseStates();

        void Start()
        {
            Anim = GetComponent<Animator>();
            Grid = GameObject.Find("Grid").GetComponent<Grid>();
            WorldManager = GameObject.Find("WorldManager").GetComponent<WorldManager>();
            Sword = GetComponentInChildren<Sword>();
            Health = GetComponent<SkeletonHealth>();
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