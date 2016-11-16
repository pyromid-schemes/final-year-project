﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Parsing.Commands;

namespace Parsing.Parsers
{
	public class BuildCommandParser : Parser
	{
		public bool CanParse (string msg)
		{
			var obj = (JObject)JsonConvert.DeserializeObject (msg);
			return CommandTypeExtensions.Parse (obj ["command"].ToString ()) == CommandType.BUILD;
		}

		public Command Parse (string msg)
		{
			var obj = (JObject)JsonConvert.DeserializeObject (msg);
			var options = (JObject)obj ["options"];

			return new BuildCommand (options["objectName"].ToString(), CommandType.BUILD);
		}
	}
}