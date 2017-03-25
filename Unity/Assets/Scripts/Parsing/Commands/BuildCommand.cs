using UnityEngine;
using System.Collections;
/*
    @author Jamie Redding (jgr2)
*/

namespace Parsing.Commands
{
	public class BuildCommand : Command
	{
		private string objectId;
		private int xPos;
		private int zPos;
		private int rot;

		public BuildCommand (string objectId, int xPos, int zPos, int rot)
		{
			this.objectId = objectId;
			this.xPos = xPos;
			this.zPos = zPos;
			this.rot = rot;
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

		public int GetRot()
		{
			return rot;
		}

		public CommandType GetCommandType()
		{
			return CommandType.BUILD;
		}
	}
}