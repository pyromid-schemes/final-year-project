using NUnit.Framework;
using System;
using Parsing.Commands;
using Newtonsoft.Json;
using Test.Builders;

namespace Test.Parsing.Commands
{
	public class CommandResolverTest
	{
		private CommandResolver underTest;
		private StubSpawner stubSp;

		[SetUp]
		public void Setup()
		{
			stubSp = new StubSpawner ();
			underTest = new CommandResolver (stubSp);
		}

		[Test]
		public void WhenReceivesParseableCorrectFormattedBuildCommandCallsAddRoomPrefab()
		{
			string msg = BuildCommandBuilder.ADefaultBuildCommandBuilder ()
				.Build ();

			stubSp.SetExpected ("default", 0, 0);

			underTest.ResolveMessage (msg);

			Assert.IsTrue (stubSp.WasCalled ());
		}

		[Test]
		public void WhenReceivesParseableButNotAValidCommandShouldNotCallSpawner()
		{
			string msg = "{\"test\":1}";

			underTest.ResolveMessage (msg);

			Assert.IsFalse (stubSp.WasCalled ());
		}

		[Test]
		public void WhenReceivesNonJsonShouldNotCallSpawner()
		{
			string msg = "££";

			underTest.ResolveMessage (msg);

			Assert.IsFalse (stubSp.WasCalled ());
		}

		private class StubSpawner : Spawn.ISpawner
		{
			private bool called = false;
			
			private string expectedObjectId;
			private int expectedXPos;
			private int expectedZPos;
			
			public void AddRoomPrefab (string objectId, int xPos, int zPos)
			{
				called = true;

				Assert.AreEqual (expectedObjectId, objectId);
				Assert.AreEqual (expectedXPos, xPos);
				Assert.AreEqual (expectedZPos, zPos);
			}

			public void SetExpected (string objectId, int xPos, int zPos)
			{
				expectedObjectId = objectId;
				expectedXPos = xPos;
				expectedZPos = zPos;
			}

			public bool WasCalled()
			{
				return called;
			}
		}
	}
}

