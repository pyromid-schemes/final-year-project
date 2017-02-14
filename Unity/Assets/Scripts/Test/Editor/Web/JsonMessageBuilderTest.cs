using System;
using NUnit.Framework;
using Web;
using UnityEngine;
using World;

namespace Test.Web
{
	public class JsonMessageBuilderTest
	{

		public JsonMessageBuilderTest ()
		{
		}

		[Test]
		public void FormatVRPositionIntoJsonVRPositionMessage ()
		{
			string expected = "\"vrPosition\": {" +
				"\"xPos\":10," +
				"\"zPos\":20" +
				"}";

			Assert.AreEqual (expected, JsonMessageBuilder.FormatVRPosition (new Vector3 (10, 0, -20)));
		}

//		[Test]
//		public void FormatRoomIntoWorldStatusMessage ()
//		{
//			string expected = "{" +
//			                  "\"objectId\":\"room1\"," +
//			                  "\"xPos\":10," +
//			                  "\"zPos\":20," +
//			                  "\"rot\":0" +
//			                  "}";
//
//			Assert.AreEqual (expected, JsonMessageBuilder.FormatRoom (new PlacedPrefab ("room1", new Vector3 (10, 0, -20), Quaternion.identity)));
//		}

//		[Test]
//		public void FormatSingleMobAsJsonMessage ()
//		{
//			string expected = "{" +
//				"\"objectId\":\"mobKnight\"," +
//				"\"xPos\":10," +
//				"\"zPos\":20," +
//				"\"id\":0," +
//				"\"dead\":false" +
//				"}";
//			GameObject mob = new GameObject ("test");
//			mob.transform.Translate (10, 0, -20);
//
//			Assert.AreEqual (expected, JsonMessageBuilder.FormatMob (new PlacedMob ("mobKnight", 0, mob)));
//		}
	}
}