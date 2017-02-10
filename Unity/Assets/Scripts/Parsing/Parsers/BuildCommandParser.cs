using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Parsing.Commands;
using System;

namespace Parsing.Parsers
{
	public class BuildCommandParser : Parser
	{
		public bool CanParse (string msg)
		{
			var obj = (JObject)JsonConvert.DeserializeObject (msg);
			if (obj ["command"] == null) {
				return false;
			}

			var options = (JObject)obj ["options"];
			if (options ["objectId"] == null) {
				return false;
			}

			int result = 0;
			if (options ["xPos"] == null || !Int32.TryParse (options ["xPos"].ToString (), out result)) {
				return false;
			}

			if (options ["zPos"] == null || !Int32.TryParse (options ["zPos"].ToString (), out result)) {
				return false;
			}

			if (options ["rot"] == null || !Int32.TryParse (options ["rot"].ToString (), out result)) {
				return false;
			}

			return CommandTypeExtensions.Parse (obj ["command"].ToString ()) == CommandType.BUILD;
		}

		public Command Parse (string msg)
		{
			var obj = (JObject)JsonConvert.DeserializeObject (msg);
			var options = (JObject)obj ["options"];

			return new BuildCommand (options["objectId"].ToString(), 
				Int32.Parse(options["xPos"].ToString()), 
				Int32.Parse(options["zPos"].ToString()),
				Int32.Parse(options["rot"].ToString()));
		}
	}
}