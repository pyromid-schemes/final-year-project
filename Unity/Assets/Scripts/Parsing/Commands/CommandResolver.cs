using Parsing.Commands;
using UnityEngine;
using System;

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
				Debug.Log (String.Format("Build command received.\nObjectId:{0}, xPos:{1}, zPos{2}", 
					buildCommand.GetObjectId (), buildCommand.GetXPos(), buildCommand.GetZPos()));
				break;
			}
		}
	}
}