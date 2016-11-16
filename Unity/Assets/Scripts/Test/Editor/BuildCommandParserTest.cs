using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;
using Parsing.Commands;

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
			var expected = new BuildCommand ("ball", CommandType.BUILD);
			var actual = (BuildCommand)underTest.Parse (TestMsgWithCommandName ("build"));

			Assert.AreEqual (expected.GetObjectName (), actual.GetObjectName ());
			Assert.AreEqual (expected.GetCommandType (), actual.GetCommandType ());
		}

		private string TestMsgWithCommandName(string command)
		{
			return "{\"command\":\"" + command + "\",\"options\":{\"objectName\":\"ball\"}}";
		}
			
	}
}
