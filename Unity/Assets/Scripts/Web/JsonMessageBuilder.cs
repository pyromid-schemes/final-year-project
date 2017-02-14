using System;
using UnityEngine;
using System.Text;
using World;

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
				mob.HasBeenKilled ());
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

