using Parsing.Commands;
using UnityEngine;
using System;
using World;

namespace Parsing.Commands
{
	public class CommandResolver
	{
		private IWorldManager worldManager;
		private ParserLibrary parserLibrary;

		public CommandResolver (IWorldManager spawner)
		{
			parserLibrary = new ParserLibrary ();
			this.worldManager = spawner;
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
				worldManager.AddPrefab (buildCommand.GetObjectId (), buildCommand.GetXPos (), AntiCorruption.FixHandedness(buildCommand.GetZPos ()), buildCommand.GetRot ());
				break;
			case CommandType.SPAWNMOB:
				var spawnMobCommand = (SpawnMobCommand)command;
				worldManager.SpawnMob (spawnMobCommand.GetObjectId (), spawnMobCommand.GetXPos (), AntiCorruption.FixHandedness(spawnMobCommand.GetZPos ()), spawnMobCommand.GetId ());
				break;
			}
		}
	}
}