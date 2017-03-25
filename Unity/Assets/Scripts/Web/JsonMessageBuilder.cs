using System;
using UnityEngine;
using System.Text;
using World;
using System.Collections.Generic;
using Communication;
/*
    @author Jamie Redding (jgr2)
*/

namespace Web
{
	public class JsonMessageBuilder
	{
		private static readonly string VR_PLAYER_TEMPLATE = "\"vrPosition\": {{" +
			"\"xPos\":{0}," +
			"\"zPos\":{1}," +
			"\"rot\":{2}," +
			"\"maxHealth\":{3}," +
			"\"currentHealth\":{4}" +
			"}}";

		private static readonly string MOB_TEMPLATE = "{{" +
			"\"objectId\":\"{0}\"," +
			"{1}" +
			"\"id\":{2}," +
			"\"dead\":{3}" +
			"}}";

		private static readonly string OPTIONAL_MOB_FIELDS_TEMPLATE = 
			"\"xPos\":{0}," +
			"\"zPos\":{1}," +
			"\"rot\":{2}," +
			"\"maxHealth\":{3}," +
			"\"currentHealth\":{4},";

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

		public static string FormatPositionsMessage (GameObject vrPlayer, List<PlacedMob> mobs, List<Communication.Event> deadMobs) {
			StringBuilder jsonMobs = new StringBuilder ();
			for (int i = 0; i < mobs.Count; i++) {
				jsonMobs.Append (FormatMob (mobs [i]));
				if (deadMobs.Count > 0 || i != mobs.Count - 1) {
					jsonMobs.Append (",");
				}
			}

			for (int i = 0; i < deadMobs.Count; i++) {
				jsonMobs.Append (FormatMob (((KillMobEvent) deadMobs [i]).GetMob ()));
				if (i != deadMobs.Count - 1) {
					jsonMobs.Append (",");
				}
			}

			return string.Format (POSITIONS_TEMPLATE,
				FormatVRPlayer (vrPlayer),
				jsonMobs.ToString ());
		}

		public static string FormatVRPlayer(GameObject player)
		{
			return string.Format (VR_PLAYER_TEMPLATE, 
				player.transform.position.x,
				AntiCorruption.FixHandedness (player.transform.position.z),
				Mathf.RoundToInt (player.transform.rotation.eulerAngles.y),
				GetDamageable (player).GetMaxHealth (),
				GetDamageable (player).GetHealth ());
		}

		public static string FormatMob(PlacedMob mob)
		{
			string optionalFields = "";
			string dead;

			if (mob.HasBeenKilled ()) {
				dead = "true";
			} else {
				optionalFields = string.Format (OPTIONAL_MOB_FIELDS_TEMPLATE, 
					mob.GetGameObject ().transform.position.x,
					AntiCorruption.FixHandedness ( mob.GetGameObject ().transform.position.z),
					Mathf.RoundToInt (mob.GetGameObject ().transform.rotation.eulerAngles.y),
					GetDamageable (mob.GetGameObject ()).GetMaxHealth (),
					GetDamageable (mob.GetGameObject ()).GetHealth ());
				dead = "false";
			}

			return string.Format (MOB_TEMPLATE, 
				mob.GetName (), 
				optionalFields,
				mob.GetId (),
				dead);
		}

		public static string FormatRoom (PlacedPrefab room)
		{
			return string.Format (ROOM_TEMPLATE, 
				room.GetName (),
				room.GetPosition ().x,
				AntiCorruption.FixHandedness (room.GetPosition ().z),
				Mathf.RoundToInt(room.GetRotation ().eulerAngles.y));
		}

		public static IDamageable GetDamageable (GameObject g)
		{
			return (IDamageable)g.GetComponent (typeof(IDamageable));
		}
	}
}

