using System.Collections.Generic;
/*
    @author Jamie Redding (jgr2)
*/

namespace World
{
    public interface IWorldManager
    {
        void AddPrefab(string objectId, int xPos, int zPos, int rot);
        void SpawnMob(string objectId, float xPos, float zPos, int id);
        List<PlacedMob> GetMobs();
    }
}