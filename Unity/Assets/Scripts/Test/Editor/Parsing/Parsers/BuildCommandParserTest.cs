using NUnit.Framework;
using Parsing.Commands;
using Parsing.Parsers;
using Test.Builders;
/**
 * @author Jamie Redding
 */
namespace Test.Parsing.Parsers {
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
			Assert.IsTrue(underTest.CanParse(TestMsgWithCommandName("build")));
		}

		[Test]
		public void CannotParseUnsupportedCommand()
		{
			Assert.IsFalse (underTest.CanParse (TestMsgWithCommandName ("foobar")));
		}

		[Test]
		public void CannotParseCommandWithoutObjectIdKey()
		{
			Assert.IsFalse (underTest.CanParse ("{\"command\":\"build\",\"options\":{\"xPos\":0,\"zPos\":0,\"rot\":0}}"));
		}

		[Test]
		public void CannotParseCommandWithoutXPosKey()
		{
			Assert.IsFalse (underTest.CanParse ("{\"command\":\"build\",\"options\":{\"objectId\":\"room1\",\"zPos\":0,\"rot\":0}}"));
		}

		[Test]
		public void CannotParseCommandWithoutZPosKey()
		{
			Assert.IsFalse (underTest.CanParse ("{\"command\":\"build\",\"options\":{\"objectId\":\"room1\",\"xPos\":0,\"rot\":0}}"));
		}

		[Test]
		public void CannotParseCommandWithoutCommandKey()
		{
			Assert.IsFalse (underTest.CanParse ("{\"options\":{\"objectId\":\"room1\",\"xPos\":0,\"zPos\":0,\"rot\":0}}"));
		}

		[Test]
		public void CannotParseCommandWithoutRotationKey()
		{
			Assert.IsFalse (underTest.CanParse ("{\"command\":\"build\",\"options\":{\"objectId\":\"room1\",\"xPos\":0,\"zPos\":0}}"));
		}

		[Test]
		public void ParseBuildCommand()
		{
			var expected = new BuildCommand ("room1", 0, 0, 0);
			var actual = (BuildCommand)underTest.Parse (
				BuildCommandBuilder.ADefaultBuildCommandBuilder()
				.WithObjectId("room1")
				.WithXPos(0)
				.WithZPos(0)
				.WithRot(0)
				.Build());

			Assert.AreEqual (expected.GetObjectId (), actual.GetObjectId ());
			Assert.AreEqual (expected.GetCommandType (), actual.GetCommandType ());
			Assert.AreEqual (expected.GetXPos (), actual.GetXPos ());
			Assert.AreEqual (expected.GetZPos (), actual.GetZPos ());
			Assert.AreEqual (expected.GetRot (), actual.GetRot ());
		}

		private string TestMsgWithCommandName(string command)
		{
			return "{\"command\":\"" + command + "\",\"options\":{\"objectId\":\"room1\",\"xPos\":0,\"zPos\":0,\"rot\":0}}";
		}
			
	}
}
