using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * @author Daniel Burnley
 */
namespace AI.Pathfinding
{
    public struct Node
    {
        public readonly float X;
        public readonly float Z;

        public Node(float x, float z)
        {
            X = x;
            Z = z;
        }

        public bool Equals(Node other)
        {
            return Math.Abs(other.X - X) < 0.00001 && Math.Abs(other.Z - Z) < 0.00001;
        }
    }

    public class GridManager
    {
        private readonly SortedDictionary<float, List<float>> _nodes;
        private readonly IGrid _grid;

        public GridManager(IGrid grid)
        {
            _nodes = new SortedDictionary<float, List<float>>();
            _grid = grid;
        }

        public void AddNode(float x, float z)
        {
            if (_nodes.ContainsKey(x))
            {
                _nodes[x].Add(z);
                _nodes[x].Sort();
            }
            else
            {
                _nodes[x] = new List<float> {z};
            }
        }

        public bool IsOccupied(int id, PathfindingNode node)
        {
            var mobPositions = _grid.GetMobPositions();
            foreach (var positions in mobPositions)
            {
                if (positions.Key == id) continue;
                if (positions.Value.Contains(node)) return true;
            }
            return false;
        }

        public bool IsWalkable(float x, float z)
        {
            try
            {
                return _nodes[x].Contains(z);
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        public Node GetClosestNode(float currentX, float currentZ)
        {
            if (_nodes.ContainsKey(currentX) && _nodes[currentX].Contains(currentZ))
            {
                return new Node(currentX, currentZ);
            }
            var closestX = 0f;
            var closestXDifference = float.PositiveInfinity;
            var closestZ = 0f;
            var closestZDifference = float.PositiveInfinity;
            foreach (var x in _nodes.Keys)
            {
                var difference = Math.Max(currentX, x) - Math.Min(currentX, x);
                if (!(difference < closestXDifference)) continue;
                closestX = x;
                closestXDifference = difference;
            }
            foreach (var z in _nodes[closestX])
            {
                var difference = Math.Max(currentZ, z) - Math.Min(currentZ, z);
                if (!(difference < closestZDifference)) continue;
                closestZ = z;
                closestZDifference = difference;
            }

            return new Node(closestX, closestZ);
        }

        public SortedDictionary<float, List<float>> GetNodes()
        {
            return _nodes;
        }

        public void Debug_Dump()
        {
            var toPrint = "";
            foreach (var x in _nodes)
            {
                toPrint += x.Key + ": [";
                toPrint = x.Value.Aggregate(toPrint, (current, z) => current + (z + ", "));
                toPrint += "]\n";
            }
            Debug.Log(toPrint);
        }
    }
}