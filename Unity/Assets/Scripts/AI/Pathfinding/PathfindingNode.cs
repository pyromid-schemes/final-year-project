using System;

namespace AI.Pathfinding
{
    public class PathfindingNode : IEquatable<PathfindingNode>
    {
        public float X, Z, G, H = 0;

        public float F
        {
            get { return G + H; }
        }

        public PathfindingNode Parent;

        public PathfindingNode(float x, float z)
        {
            Parent = null;
            X = x;
            Z = z;
            G = 0;
        }

        public PathfindingNode(PathfindingNode parent, float x, float z)
        {
            X = x;
            Z = z;
            Parent = parent;
            G = parent.G + 1;
        }

        public PathfindingNode(PathfindingNode parent, float x, float z, float g)
        {
            X = x;
            Z = z;
            Parent = parent;
            G = Parent.G + g;
        }

        public bool Equals(PathfindingNode other)
        {
            return other != null && ((Math.Abs(X - other.X) < 0.1f) && (Math.Abs(Z - other.Z) < 0.1f));
        }
    }
}
