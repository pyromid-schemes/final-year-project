﻿using System.Collections;
using System.Collections.Generic;
using AI.MobControllers;
using AI.Pathfinding;
using UnityEngine;

namespace AI.States.Skeleton
{
    internal class PursueState : IState
    {
        private readonly SkeletonController _mob;
        private CalculatePath _pathCalculator;
        private GameObject _player;
        private int _nextNodeIndex;
        private List<PathfindingNode> _path;
        private Coroutine _pathfindingRoutine;

        public PursueState(SkeletonController mob)
        {
            _mob = mob;
        }

        public void OnEnter()
        {
            _nextNodeIndex = 0;
            _mob.ToWalkingState();
            _player = GameObject.Find("Player");
            CalculatePath();
            _pathfindingRoutine = _mob.StartCoroutine(CalculatePathWithWait());
        }

        public void OnUpdate()
        {
            if (!PlayerInSight())
            {
                _mob.StopCoroutine(_pathfindingRoutine);
                _mob.ChangeState(SkeletonController.States.Patrol);
            }
            else if (Vector3.Distance(_mob.transform.position, _player.transform.position) < _mob.AttackRange)
            {
                _mob.StopCoroutine(_pathfindingRoutine);
                _mob.ChangeState(SkeletonController.States.Defend);
            }
        }

        public void OnFixedUpdate()
        {
            var playerPos = _player.transform.position;

            var mobPos = _mob.transform.position;
            _mob.transform.LookAt(new Vector3(playerPos.x, mobPos.y, playerPos.z));
            CheckIfAtNode(mobPos);
            var nextNode = _path[_nextNodeIndex];
            _mob.transform.position = Vector3.MoveTowards(mobPos,
                new Vector3(nextNode.X, mobPos.y, nextNode.Z), 1f * Time.deltaTime);
        }

        public List<PathfindingNode> GetPath()
        {
            return _path;
        }

        private void CheckIfAtNode(Vector3 mobPos)
        {
            var nextNode = new Node(_path[_nextNodeIndex].X, _path[_nextNodeIndex].Z);
            var mobNode = _mob.Grid.GetGrid().GetClosestNode(mobPos.x, mobPos.z);
            if (nextNode.Equals(mobNode)) _nextNodeIndex++;
        }

        private IEnumerator CalculatePathWithWait()
        {
            for (;;)
            {
                Profiler.BeginSample("Pathfinding");
                CalculatePath();
                Profiler.EndSample();
                yield return new WaitForSeconds(1f);
            }
        }

        private void CalculatePath()
        {
            _pathCalculator = new CalculatePath(_mob.Grid.GetGrid());
            _path = _pathCalculator.GetPathToDestination(_mob.transform.position.x,
                _mob.transform.position.z, _player.transform.position.x, _player.transform.position.z);
            _nextNodeIndex = 0;
        }

        private bool PlayerInSight()
        {
            var playerPos = _player.transform.position;
            RaycastHit hit;
            var direction = playerPos - _mob.transform.position;
            var selfPos = _mob.transform.position;
            selfPos.y += 1f;
            var ret = Physics.Raycast(_mob.transform.position, direction, out hit, _mob.MaxSightRange) &&
                      hit.collider.CompareTag("Player");
            return ret;
        }
    }
}