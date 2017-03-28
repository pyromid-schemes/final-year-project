using System.Collections.Generic;
using Parsing.Parsers;
using Newtonsoft.Json;
using UnityEngine;
using System;
/*
    @author Jamie Redding (jgr2)
*/

namespace Parsing
{
	public class ParserLibrary
	{
		private List<Parser> parsers;
		
		public ParserLibrary ()
		{
			parsers = new List<Parser> ();
			parsers.Add (new BuildCommandParser ());
			parsers.Add (new SpawnMobCommandParser ());
		}

		public Command Parse(string msg)
		{
			foreach (Parser p in parsers) {
				bool canParse = false;
				try {
					canParse = p.CanParse (msg);
				} catch (JsonReaderException ex) {
					Console.Error.WriteLine ("Message is not valid JSON: " + msg);
					Console.Error.WriteLine (ex.ToString ());
					break;
				}
				if (canParse) {
					return p.Parse (msg);
				}
			}
			return null;
		}
	}
}