using System.Collections.Generic;

namespace AI.Pathfinding
{
    public interface IGrid
    {
        Dictionary<int, HashSet<PathfindingNode>> GetMobPositions();
    }
}