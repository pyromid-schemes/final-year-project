using System;

namespace Spawn
{
	public interface ISpawner
	{
		void AddRoomPrefab (string objectId, int xPos, int zPos);
	}
}

