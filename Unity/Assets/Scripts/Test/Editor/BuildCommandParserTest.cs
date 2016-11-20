using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;
using Parsing.Commands;
using Test.Builders;

namespace Parsing.Parsers {
	public class BuildCommandParserTest
	{
		private BuildCommandParser underTest;

		[SetUp]
		public void Setup() {
			underTest = new BuildCommandParser ();
		}

		[Test]
		public void CanParseBuildCommand ()
		{
			Assert.True(underTest.CanParse(TestMsgWithCommandName("build")));
		}

		[Test]
		public void CannotParseUnsupportedCommand()
		{
			Assert.False (underTest.CanParse (TestMsgWithCommandName("foobar")));
		}

		[Test]
		public void ParseBuildCommand()
		{
			var expected = new BuildCommand ("room1", 0, 0);
			var actual = (BuildCommand)underTest.Parse (
				BuildCommandBuilder.ADefaultBuildCommandBuilder()
				.WithObjectId("room1")
				.WithXPos(0)
				.WithZPos(0)
				.Build());

			Assert.AreEqual (expected.GetObjectId (), actual.GetObjectId ());
			Assert.AreEqual (expected.GetCommandType (), actual.GetCommandType ());
			Assert.AreEqual (expected.GetXPos (), actual.GetXPos ());
			Assert.AreEqual (expected.GetZPos (), actual.GetZPos ());
		}

		private string TestMsgWithCommandName(string command)
		{
			return "{\"command\":\"" + command + "\",\"options\":{\"objectId\":\"room1\",\"xPos\":0,\"zPos\":0}}";
		}
			
	}
}
