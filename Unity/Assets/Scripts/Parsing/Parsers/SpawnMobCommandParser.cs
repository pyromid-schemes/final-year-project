using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Parsing.Commands;
/*
    @author Jamie Redding (jgr2)
*/

namespace Parsing.Parsers
{
	public class SpawnMobCommandParser : Parser
	{
		public SpawnMobCommandParser()
		{
			JsonConvert.DeserializeObject ("{\"command\":\"spawnMob\"," +
				"\"options\": {" +
				"\"objectId\": \"objectId\"," +
				"\"xPos\": 0," +
				"\"zPos\": 0," +
				"\"id\": 0" +
				"}}");
		}

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

			float result = 0;
			if (options ["xPos"] == null || !float.TryParse (options ["xPos"].ToString (), out result)) {
				return false;
			}

			if (options ["zPos"] == null || !float.TryParse (options ["zPos"].ToString (), out result)) {
				return false;
			}

			int idResult = 0;
			if (options ["id"] == null || !Int32.TryParse (options ["id"].ToString (), out idResult)) {
				return false;
			}

			return CommandTypeExtensions.Parse (obj ["command"].ToString ()) == CommandType.SPAWNMOB;
		}

		public Command Parse (string msg)
		{
			var obj = (JObject)JsonConvert.DeserializeObject (msg);
			var options = (JObject)obj ["options"];

			return new SpawnMobCommand (options ["objectId"].ToString (),
				float.Parse (options ["xPos"].ToString ()),
				float.Parse (options ["zPos"].ToString ()),
				Int32.Parse (options ["id"].ToString ()));
		}
	}
}

