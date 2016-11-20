using UnityEngine;
using System.Collections;

namespace Parsing.Commands
{
	public class BuildCommand : Command
	{
		private string objectId;
		private int xPos;
		private int zPos;

		public BuildCommand (string objectId, int xPos, int zPos)
		{
			this.objectId = objectId;
			this.xPos = xPos;
			this.zPos = zPos;
		}

		public string GetObjectId ()
		{
			return objectId;
		}

		public int GetXPos()
		{
			return xPos;
		}

		public int GetZPos()
		{
			return zPos;
		}

		public CommandType GetCommandType()
		{
			return CommandType.BUILD;
		}
	}
}