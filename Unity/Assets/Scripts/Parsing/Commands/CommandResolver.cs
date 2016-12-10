using Parsing.Commands;
using UnityEngine;
using System;
using World;

namespace Parsing.Commands
{
	public class CommandResolver
	{
		private IWorldManager spawner;
		private ParserLibrary parserLibrary;

		public CommandResolver (IWorldManager spawner)
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