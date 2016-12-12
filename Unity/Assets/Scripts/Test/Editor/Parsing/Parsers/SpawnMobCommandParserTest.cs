using NUnit.Framework;
using Parsing.Commands;
using Parsing.Parsers;
using Test.Builders;

namespace Test.Parsing.Parsers {
	public class SpawnMobCommandParserTest
	{
		private SpawnMobCommandParser underTest;

		[SetUp]
		public void Setup() {
			underTest = new SpawnMobCommandParser ();
		}

		[Test]
		public void CanParseBuildCommand ()
		{
			Assert.IsTrue(underTest.CanParse(TestMsgWithCommandName("spawnMob")));
		}

		[Test]
		public void CannotParseUnsupportedCommand()
		{
			Assert.IsFalse (underTest.CanParse (TestMsgWithCommandName ("foobar")));
		}

		[Test]
		public void CannotParseCommandWithoutObjectIdKey()
		{
			Assert.IsFalse (underTest.CanParse ("{\"command\":\"spawnMob\",\"options\":{\"xPos\":0,\"zPos\":0,\"id\":0}}"));
		}

		[Test]
		public void CannotParseCommandWithoutXPosKey()
		{
			Assert.IsFalse (underTest.CanParse ("{\"command\":\"spawnMob\",\"options\":{\"objectId\":\"room1\",\"zPos\":0,\"id\":0}}"));
		}

		[Test]
		public void CannotParseCommandWithoutZPosKey()
		{
			Assert.IsFalse (underTest.CanParse ("{\"command\":\"spawnMob\",\"options\":{\"objectId\":\"room1\",\"xPos\":0,\"id\":0}}"));
		}

		[Test]
		public void CannotParseCommandWithoutCommandKey()
		{
			Assert.IsFalse (underTest.CanParse ("{\"options\":{\"objectId\":\"room1\",\"xPos\":0,\"zPos\":0,\"id\":0}}"));
		}

		[Test]
		public void CannotParseCommandWithoutIdKey()
		{
			Assert.IsFalse (underTest.CanParse ("{\"command\":\"spawnMob\",\"options\":{\"objectId\":\"room1\",\"xPos\":0,\"zPos\":0}}"));
		}

		[Test]
		public void CannotParseCommandWithNonFloatXPos()
		{
			Assert.IsFalse (underTest.CanParse ("{\"command\":\"spawnMob\",\"options\":{\"objectId\":\"room1\",\"xPos\":\"jjj\",\"zPos\":0,\"id\":0}}"));
		}

		[Test]
		public void CannotParseCommandWithNonFloatZPos()
		{
			Assert.IsFalse (underTest.CanParse ("{\"command\":\"spawnMob\",\"options\":{\"objectId\":\"room1\",\"xPos\":0,\"zPos\":\"jjj\",\"id\":0}}"));
		}

		[Test]
		public void CannotParseCommandWithNonIntId()
		{
			Assert.IsFalse (underTest.CanParse ("{\"command\":\"spawnMob\",\"options\":{\"objectId\":\"room1\",\"xPos\":0,\"zPos\":0,\"id\":\"dd\"}}"));
		}

		[Test]
		public void ParseBuildCommand()
		{
			var expected = new SpawnMobCommand ("knight", 0f, 0f, 0);
			var actual = (SpawnMobCommand)underTest.Parse (
				SpawnMobCommandBuilder.ADefaultSpawnMobCommand ()
				.WithObjectId ("knight")
				.WithXPos (0f)
				.WithZPos (0f)
				.WithId (0)
				.Build ());

			Assert.AreEqual (expected.GetObjectId (), actual.GetObjectId ());
			Assert.AreEqual (expected.GetCommandType (), actual.GetCommandType ());
			Assert.AreEqual (expected.GetXPos (), actual.GetXPos ());
			Assert.AreEqual (expected.GetZPos (), actual.GetZPos ());
			Assert.AreEqual (expected.GetId (), actual.GetId ());
		}

		private string TestMsgWithCommandName(string command)
		{
			return "{\"command\":\"" + command + "\",\"options\":{\"objectId\":\"knight\",\"xPos\":0,\"zPos\":0,\"id\":0}}";
		}

	}
}
