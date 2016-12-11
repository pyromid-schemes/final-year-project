using System;
using Parsing;

namespace Parsing.Commands
{
	public class SpawnMobCommand : Command
	{

		private string objectId;
		private float xPos;
		private float zPos;
		private int id;

		public SpawnMobCommand (string objectId, float xPos, float zPos, int id)
		{
			this.objectId = objectId;
			this.xPos = xPos;
			this.zPos = zPos;
			this.id = id;
		}

		public string GetObjectId ()
		{
			return objectId;
		}

		public float GetXPos()
		{
			return xPos;
		}

		public float GetZPos()
		{
			return zPos;
		}

		public int GetId()
		{
			return id;
		}

		public CommandType GetCommandType()
		{
			return CommandType.SPAWNMOB;
		}
	}
}

