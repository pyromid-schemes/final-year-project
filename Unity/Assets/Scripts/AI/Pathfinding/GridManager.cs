using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AI.Pathfinding
{
    public struct Node
    {
        public readonly float x;
        public readonly float z;

        public Node(float x, float z)
        {
            this.x = x;
            this.z = z;
        }
    }

    public class GridManager
    {
        private readonly SortedDictionary<float, List<float>> _nodes;

        public GridManager()
        {
            _nodes = new SortedDictionary<float, List<float>>();
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

        public bool IsWalkable(float x, float z)
        {
            try
            {
                return _nodes[x].Contains(z);
            }
            catch (KeyNotFoundException e)
            {
                return false;
            }
        }

        public Node GetClosestNode(float currentX, float currentZ)
        {
            if(_nodes.ContainsKey(currentX) && _nodes[currentX].Contains(currentZ))
            {
                return new Node(currentX, currentZ);
            }
            float closestX = 0f;
            float closestXDifference = Single.PositiveInfinity;
            float closestZ = 0f;
            float closestZDifference = Single.PositiveInfinity;
            foreach (float x in _nodes.Keys)
            {
                float difference = Math.Abs(currentX - x);
                if (difference < closestXDifference)
                { 
                    closestX = x;
                    closestXDifference = difference;
                }
                else
                {
                    break;
                }
            }
            foreach (float z in _nodes[closestX])
            {
                float difference = Math.Abs(currentZ - z);
                if (difference < closestZDifference)
                {
                    closestZ = z;
                    closestZDifference = difference;
                }
                else
                {
                    break;
                }
            }

            return new Node(closestX, closestZ);
        }

        public SortedDictionary<float, List<float>> GetNodes()
        {
            return _nodes;
        }
    }
}