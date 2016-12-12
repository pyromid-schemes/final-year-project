using UnityEngine;
using System.Collections;
using System;

namespace Parsing
{
	public static class CommandTypeExtensions {
		public static CommandType Parse(string s) {
			try {
				return (CommandType)Enum.Parse (typeof(CommandType), s.ToUpper());
			}
			catch (ArgumentException) {
				return CommandType.TYPE_NOT_FOUND;
			}
		}
	}

	public enum CommandType {
		BUILD, SPAWNMOB, TYPE_NOT_FOUND
	}
}