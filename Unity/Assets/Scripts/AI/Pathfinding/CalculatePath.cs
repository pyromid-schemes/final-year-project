using System;
using System.Collections.Generic;

namespace AI.Pathfinding
{
    public class CalculatePath
    {
        private readonly GridManager _grid;
        private readonly int _id;
        private readonly Dictionary<int, PathfindingNode> _openList;
        private readonly HashSet<PathfindingNode> _closedList;

        private bool _atDestination;
        private PathfindingNode _startingNode;
        private PathfindingNode _destinationNode;
        private PathfindingNode _currentNode;
        private const float MaxDistance = 12.5f;

        public CalculatePath(GridManager grid, int id)
        {
            _id = id;
            _grid = grid;
            _openList = new Dictionary<int, PathfindingNode>();
            _closedList = new HashSet<PathfindingNode>();
        }

        public List<PathfindingNode> GetPathToDestination(float startingX, float startingZ, float destinationX,
            float destinationZ)
        {
            _openList.Clear();
            _closedList.Clear();
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
            var possibleNodes = new List<PathfindingNode>
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
                if (!NodeIsWalkable(possibleNode)) continue;
                possibleNode.H = CalculateH(possibleNode);
                var existingOpenNode = FindNodeInOpenList(possibleNode);
                if (existingOpenNode != null)
                {
                    if (!(possibleNode.F < existingOpenNode.F)) continue;
                    _openList.Remove(existingOpenNode.GetHashCode());
                    _openList.Add(possibleNode.GetHashCode(), possibleNode);
                }
                else
                {
                    _openList.Add(possibleNode.GetHashCode(), possibleNode);
                }
            }
        }

        private bool NodeIsWalkable(PathfindingNode possibleNode)
        {
            if (possibleNode.G > MaxDistance / Grid.SpaceBetween) return false;
            var isStart = possibleNode.Equals(_startingNode);
            if (isStart) return false;
            var isWalkable = _grid.IsWalkable(possibleNode.X, possibleNode.Z);
            if (!isWalkable) return false;
            var isOccupied = _grid.IsOccupied(_id, possibleNode);
            if (isOccupied) return false;
            var nodeInClosedList = NodeInClosedList(possibleNode);
            return !nodeInClosedList;
        }

        private void GoToNextNode()
        {
            if (_openList.Count == 0)
            {
                _atDestination = true;
                return;
            }
            PathfindingNode closest = null;
            foreach (var node in _openList.Values)
            {
                if (closest == null)
                    closest = node;
                else if (node.F < closest.F)
                    closest = node;
            }
            if (closest == null)
            {
                throw new Exception("Who knows how this happened. Something must have broke somewhere");
            }
            _openList.Remove(closest.GetHashCode());
            _closedList.Add(closest);
            _currentNode = closest;
        }

        private bool NodeInClosedList(PathfindingNode node)
        {
            return _closedList.Contains(node);
        }

        private PathfindingNode FindNodeInOpenList(PathfindingNode node)
        {
            PathfindingNode ret = null;
            if (_openList.ContainsKey(node.GetHashCode()))
            {
                ret = _openList[node.GetHashCode()];
            }
            return ret;
        }

        // I mean this is really just an approximation but what can you do, eh
        // Pythagoras comes to save the day once again
        private PathfindingNode GetNodeClosestToDestination()
        {
            var cloestDistance = float.PositiveInfinity;
            var closestNode = _startingNode;
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
                return new List<PathfindingNode>();
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