using System;
using NUnit.Framework;
using Web;
using UnityEngine;
using World;

namespace Test.Web
{
	public class JsonMessageBuilderTest
	{

		[Test]
		public void FormatVRPositionIntoJsonVRPositionMessage ()
		{
			string expected = "\"vrPosition\": {" +
				"\"xPos\":10," +
				"\"zPos\":20" +
				"}";

			Assert.AreEqual (expected, JsonMessageBuilder.FormatVRPosition (new Vector3 (10, 0, -20)));
		}
	}
}