using Parsing.Commands;
using UnityEngine;
using System;
using Spawn;

namespace Parsing.Commands
{
	public class CommandResolver : MonoBehaviour
	{
		public Spawner spawner;

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
				spawner.addRoomPrefab (buildCommand.GetObjectId (), buildCommand.GetXPos (), buildCommand.GetZPos ());
				break;
			}
		}
	}
}