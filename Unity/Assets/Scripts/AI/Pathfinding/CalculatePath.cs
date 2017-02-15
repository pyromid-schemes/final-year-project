using System;
using System.CodeDom;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Pathfinding
{
    public class CalculatePath
    {
        private readonly GridManager _grid;
        private List<PathfindingNode> _openList;
        private List<PathfindingNode> _closedList;
        private bool _atDestination = false;
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
            Node closestStartingNode = _grid.GetClosestNode(startingX, startingZ);
            Node closestDestinationNode = _grid.GetClosestNode(destinationX, destinationZ);
            _startingNode = new PathfindingNode(closestStartingNode.x, closestStartingNode.z);
            _destinationNode = new PathfindingNode(closestDestinationNode.x, closestDestinationNode.z);
            _currentNode = _startingNode;
            if (_currentNode.Equals(_destinationNode))
            {
                return new List<PathfindingNode>() {_destinationNode};
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
                new PathfindingNode(_currentNode, _currentNode.X - Grid._spaceBetween, _currentNode.Z),
                new PathfindingNode(_currentNode, _currentNode.X + Grid._spaceBetween, _currentNode.Z),
                new PathfindingNode(_currentNode, _currentNode.X, _currentNode.Z - Grid._spaceBetween),
                new PathfindingNode(_currentNode, _currentNode.X, _currentNode.Z + Grid._spaceBetween)
            };

            foreach (PathfindingNode possibleNode in possibleNodes)
            {
                if (!possibleNode.Equals(_startingNode) && _grid.IsWalkable(possibleNode.X, possibleNode.Z) &&
                    !NodeInClosedList(possibleNode))
                {
                    possibleNode.H = CalculateH(possibleNode);
                    PathfindingNode existingOpenNode = FindNodeInOpenList(possibleNode);
                    if (existingOpenNode != null)
                    {
                        if (possibleNode.F < existingOpenNode.F)
                        {
                            _openList.Remove(existingOpenNode);
                            _openList.Add(possibleNode);
                        }
                    }
                    else
                    {
                        _openList.Add(possibleNode);
                    }
                }
            }
        }

        private void GoToNextNode()
        {
            PathfindingNode closest = _openList[0];
            foreach (PathfindingNode node in _openList)
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
            foreach (PathfindingNode closedNode in _closedList)
            {
                if (node.Equals(closedNode))
                {
                    return true;
                }
            }
            return false;
        }

        private PathfindingNode FindNodeInOpenList(PathfindingNode node)
        {
            foreach (PathfindingNode openNode in _openList)
            {
                if (node.Equals(openNode))
                {
                    return openNode;
                }
            }
            return null;
        }

        private List<PathfindingNode> GetPath()
        {
            List<PathfindingNode> path = new List<PathfindingNode>();
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