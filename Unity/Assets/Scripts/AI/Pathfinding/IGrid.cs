using System.Collections.Generic;

namespace AI.Pathfinding
{
    public interface IGrid
    {
        SortedDictionary<float, List<float>> GetMobPositions();
    }
}