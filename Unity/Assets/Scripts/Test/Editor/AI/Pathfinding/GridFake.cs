using System.Collections.Generic;
using AI.Pathfinding;

namespace Test.AI.Pathfinding
{
    public class GridFake : IGrid
    {

        public SortedDictionary<float, List<float>> OccupiedNodes;

        public SortedDictionary<float, List<float>> GetMobPositions()
        {
            return OccupiedNodes ?? new SortedDictionary<float, List<float>>();
        }
    }
}