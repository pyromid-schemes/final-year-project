using Parsing.Commands;
using UnityEngine;
using System;
using Spawn;

namespace Parsing.Commands
{
	public class CommandResolver
	{
		private ISpawner spawner;
		private ParserLibrary parserLibrary;

		public CommandResolver (ISpawner spawner)
		{
			parserLibrary = new ParserLibrary ();
			this.spawner = spawner;
		}

		public void ResolveMessage(string msg)
		{
			Command command = parserLibrary.Parse (msg);
			if (command == null) {
				return;
			}

			switch (command.GetCommandType ()) {
			case CommandType.BUILD:
				var buildCommand = (BuildCommand)command;
				spawner.AddPrefab (buildCommand.GetObjectId (), buildCommand.GetXPos (), buildCommand.GetZPos ());
				break;
			}
		}
	}
}