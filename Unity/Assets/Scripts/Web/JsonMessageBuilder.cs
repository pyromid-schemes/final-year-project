using System;
using UnityEngine;
using System.Text;
using World;
using System.Collections.Generic;

namespace Web
{
	public class JsonMessageBuilder
	{
		private static readonly string VR_POSITION_TEMPLATE = "\"vrPosition\": {{" +
			"\"xPos\":{0}," +
			"\"zPos\":{1}" +
			"}}";

		private static readonly string MOB_TEMPLATE = "{{" +
			"\"objectId\":\"{0}\"," +
			"\"xPos\":{1}," +
			"\"zPos\":{2}," +
			"\"id\":{3}," +
			"\"dead\":{4}" +
			"}}";

		private static readonly string ROOM_TEMPLATE = "{{" +
			"\"objectId\":\"{0}\"," +
			"\"xPos\":{1}," +
			"\"zPos\":{2}," +
			"\"rot\":{3}" +
			"}}";

		private static readonly string WORLD_STATUS_TEMPLATE = "{{" +
			"\"command\":\"worldStatus\"," +
			"\"objects\":[{0}]" +
			"}}";

		private static readonly string POSITIONS_TEMPLATE = "{{" +
			"\"command\":\"positions\"," +
			"{0}," +
			"\"mobs\":[{1}]" +
			"}}";

		public static string FormatWorldStatusMessage (List<PlacedPrefab> rooms)
		{
			StringBuilder jsonRooms = new StringBuilder ();
			for (int i = 0; i < rooms.Count; i++) {
				jsonRooms.Append (FormatRoom (rooms [i]));
				if (i != rooms.Count - 1) {
					jsonRooms.Append (",");
				}
			}

			return string.Format (WORLD_STATUS_TEMPLATE,
				jsonRooms.ToString ());
		}

		public static string FormatPositionsMessage (Vector3 vrPosition, List<PlacedMob> mobs) {
			StringBuilder jsonMobs = new StringBuilder ();
			for (int i = 0; i < mobs.Count; i++) {
				jsonMobs.Append (FormatMob (mobs [i]));
				if (i != mobs.Count - 1) {
					jsonMobs.Append (",");
				}
			}

			return string.Format (POSITIONS_TEMPLATE,
				FormatVRPosition (vrPosition),
				jsonMobs.ToString ());
		}

		public static string FormatVRPosition(Vector3 position)
		{
			return string.Format (VR_POSITION_TEMPLATE, 
				position.x,
				AntiCorruption.FixHandedness (position.z));
		}

		public static string FormatMob(PlacedMob mob)
		{
			return string.Format (MOB_TEMPLATE, 
				mob.GetName (), 
				mob.GetGameObject ().transform.position.x,
				AntiCorruption.FixHandedness (mob.GetGameObject ().transform.position.z),
				mob.GetId (),
				mob.ShouldKillMobOnWeb () ? "true" : "false");
		}

		public static string FormatRoom (PlacedPrefab room)
		{
			return string.Format (ROOM_TEMPLATE, 
				room.GetName (),
				room.GetPosition ().x,
				AntiCorruption.FixHandedness (room.GetPosition ().z),
				Mathf.RoundToInt(room.GetRotation ().eulerAngles.y));
		}
	}
}

