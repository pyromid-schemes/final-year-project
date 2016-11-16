using UnityEngine;
using System.Collections;

namespace Parsing.Commands
{
	public class BuildCommand : Command
	{
		private string objectName;
		private CommandType type;

		public BuildCommand (string objectName, CommandType type)
		{
			this.objectName = objectName;
			this.type = type;
		}

		public string GetObjectName ()
		{
			return objectName;
		}

		public CommandType GetCommandType() {
			return type;
		}
	}
}