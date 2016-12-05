using System;

namespace Spawn
{
	public interface ISpawner
	{
		void AddPrefab (string objectId, int xPos, int zPos);
	}
}

