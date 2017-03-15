using System.Collections.Generic;
using AI.Pathfinding;

namespace Test.AI.Pathfinding
{
    public class GridFake : IGrid
    {

        public Dictionary<int, HashSet<PathfindingNode>> OccupiedNodes;

        public Dictionary<int, HashSet<PathfindingNode>> GetMobPositions()
        {
            return OccupiedNodes ?? new Dictionary<int, HashSet<PathfindingNode>>();
        }
    }
}