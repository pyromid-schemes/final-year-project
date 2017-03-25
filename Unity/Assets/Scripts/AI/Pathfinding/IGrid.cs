using System.Collections.Generic;

/*
 * @author Daniel Burnley
 */
namespace AI.Pathfinding
{
    public interface IGrid
    {
        Dictionary<int, HashSet<PathfindingNode>> GetMobPositions();
    }
}