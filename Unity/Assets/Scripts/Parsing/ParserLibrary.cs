using System.Collections.Generic;
using Parsing.Parsers;

namespace Parsing
{
	public class ParserLibrary
	{
		private List<Parser> parsers;
		
		public ParserLibrary ()
		{
			parsers = new List<Parser> ();
			parsers.Add (new BuildCommandParser());
		}

		public Command Parse(string msg)
		{
			foreach (Parser p in parsers) {
				if (p.CanParse (msg)) {
					return p.Parse (msg);
				}
			}
			return null;
		}
	}
}