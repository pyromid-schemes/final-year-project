using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI.Pathfinding
{
    public class CalculatePath
    {
        private readonly GridManager _grid;
        private List<PathfindingNode> _openList;
        private List<PathfindingNode> _closedList;
        private bool _atDestination;
        private PathfindingNode _startingNode;
        private PathfindingNode _destinationNode;
        private PathfindingNode _currentNode;

        public CalculatePath(GridManager grid)
        {
            _grid = grid;
            _openList = new List<PathfindingNode>();
            _closedList = new List<PathfindingNode>();
        }

        public List<PathfindingNode> GetPathToDestination(float startingX, float startingZ, float destinationX,
            float destinationZ)
        {
            _openList = new List<PathfindingNode>();
            _closedList = new List<PathfindingNode>();
            var closestStartingNode = _grid.GetClosestNode(startingX, startingZ);
            var closestDestinationNode = _grid.GetClosestNode(destinationX, destinationZ);
            _startingNode = new PathfindingNode(closestStartingNode.X, closestStartingNode.Z);
            _destinationNode = new PathfindingNode(closestDestinationNode.X, closestDestinationNode.Z);
            _currentNode = _startingNode;
            if (_currentNode.Equals(_destinationNode))
            {
                return new List<PathfindingNode> {_destinationNode};
            }
            while (!_atDestination)
            {
                AddAdjacentNodesToOpenList();
                GoToNextNode();
                if (_currentNode.Equals(_destinationNode))
                {
                    _atDestination = true;
                }
            }
            return GetPath();
        }

        private void AddAdjacentNodesToOpenList()
        {
            List<PathfindingNode> possibleNodes = new List<PathfindingNode>
            {
                // Up down left right
                new PathfindingNode(_currentNode, _currentNode.X - Grid.SpaceBetween, _currentNode.Z),
                new PathfindingNode(_currentNode, _currentNode.X + Grid.SpaceBetween, _currentNode.Z),
                new PathfindingNode(_currentNode, _currentNode.X, _currentNode.Z - Grid.SpaceBetween),
                new PathfindingNode(_currentNode, _currentNode.X, _currentNode.Z + Grid.SpaceBetween),
                // Diagonals
                new PathfindingNode(_currentNode, _currentNode.X - Grid.SpaceBetween,
                    _currentNode.Z + Grid.SpaceBetween, 1.41f),
                new PathfindingNode(_currentNode, _currentNode.X + Grid.SpaceBetween,
                    _currentNode.Z + Grid.SpaceBetween, 1.41f),
                new PathfindingNode(_currentNode, _currentNode.X - Grid.SpaceBetween,
                    _currentNode.Z - Grid.SpaceBetween, 1.41f),
                new PathfindingNode(_currentNode, _currentNode.X + Grid.SpaceBetween,
                    _currentNode.Z - Grid.SpaceBetween, 1.41f)
            };

            foreach (var possibleNode in possibleNodes)
            {
                if (NodeIsNotWalkable(possibleNode)) continue;
                possibleNode.H = CalculateH(possibleNode);
                var existingOpenNode = FindNodeInOpenList(possibleNode);
                if (existingOpenNode != null)
                {
                    if (!(possibleNode.F < existingOpenNode.F)) continue;
                    _openList.Remove(existingOpenNode);
                    _openList.Add(possibleNode);
                }
                else
                {
                    _openList.Add(possibleNode);
                }
            }
        }

        private bool NodeIsNotWalkable(PathfindingNode possibleNode)
        {
            var ret = possibleNode.Equals(_startingNode) || !_grid.IsWalkable(possibleNode.X, possibleNode.Z) ||
                   NodeInClosedList(possibleNode);
            return ret;
        }

        private void GoToNextNode()
        {
            if (_openList.Count == 0)
            {
                _atDestination = true;
                return;
            }
            var closest = _openList[0];
            foreach (var node in _openList)
            {
                if (node.F < closest.F)
                {
                    closest = node;
                }
            }

            _openList.Remove(closest);
            _closedList.Add(closest);
            _currentNode = closest;
        }

        private bool NodeInClosedList(PathfindingNode node)
        {
            return _closedList.Any(node.Equals);
        }

        private PathfindingNode FindNodeInOpenList(PathfindingNode node)
        {
            return _openList.FirstOrDefault(node.Equals);
        }

        // I mean this is really just an approximation but what can you do, eh
        // Pythagoras comes to save the day once again
        private PathfindingNode GetNodeClosestToDestination()
        {
            var cloestDistance = float.PositiveInfinity;
            var closestNode = _currentNode;
            foreach (var node in _closedList)
            {
                var xDistance = Math.Abs(_destinationNode.X - node.X);
                var zDistance = Math.Abs(_destinationNode.Z - node.Z);
                var distance = xDistance * xDistance + zDistance * zDistance;
                if (!(distance < cloestDistance)) continue;
                cloestDistance = distance;
                closestNode = node;
            }
            return closestNode;
        }

        private List<PathfindingNode> GetPath()
        {
            if (!_currentNode.Equals(_destinationNode))
            {
                _currentNode = GetNodeClosestToDestination();
            }
            var path = new List<PathfindingNode>();
            while (_currentNode.Parent != null)
            {
                path.Add(_currentNode);
                _currentNode = _currentNode.Parent;
            }
            path.Reverse();
            return path;
        }

        private float CalculateH(PathfindingNode node)
        {
            return Math.Abs(node.X - _destinationNode.X) + Math.Abs(node.Z - _destinationNode.Z);
        }
    }
}