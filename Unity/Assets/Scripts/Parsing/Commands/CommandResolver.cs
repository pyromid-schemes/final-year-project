using Parsing.Commands;
using UnityEngine;

namespace Parsing.Commands
{
	public class CommandResolver
	{
		private ParserLibrary parserLibrary;

		public CommandResolver ()
		{
			parserLibrary = new ParserLibrary ();
		}

		public void ResolveMessage(string msg)
		{
			Command command = parserLibrary.Parse (msg);

			switch (command.GetCommandType ()) {
			case CommandType.BUILD:
				var buildCommand = (BuildCommand)command;
				Debug.Log ("Build command received.\nBuilding: " + buildCommand.GetObjectName ());
				break;
			}
		}
	}
}