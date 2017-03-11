using NUnit.Framework;
using System;
using System.Collections.Generic;
using Parsing.Commands;
using Newtonsoft.Json;
using Test.Builders;
using World;

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
		public void WhenReceivesParseableCorrectFormattedBuildCommandCallsAddPrefab()
		{
			string msg = BuildCommandBuilder.ADefaultBuildCommandBuilder ()
				.Build ();

			stubSp.SetExpectedForAddPrefabCall ("default", 0, 0, 0);

			underTest.ResolveMessage (msg);

			Assert.IsTrue (stubSp.WasAddPrefabCalled ());
		}

		[Test]
		public void WhenReceivesParseableCorrectFormattedSpawnMobCommandCallsSpawnMob()
		{
			string msg = SpawnMobCommandBuilder.ADefaultSpawnMobCommand ()
				.Build ();

			stubSp.SetExpectedForSpawnMobCall ("default", 0f, 0f, 0);

			underTest.ResolveMessage (msg);

			Assert.IsTrue (stubSp.WasSpawnMobCalled ());
		}

		[Test]
		public void WhenReceivesParseableButNotAValidCommandShouldNotCallSpawner()
		{
			string msg = "{\"test\":1}";

			underTest.ResolveMessage (msg);

			Assert.IsFalse (stubSp.WasAddPrefabCalled ());
		}

		[Test]
		public void WhenReceivesNonJsonShouldNotCallSpawner()
		{
			string msg = "££";

			underTest.ResolveMessage (msg);

			Assert.IsFalse (stubSp.WasAddPrefabCalled ());
		}

		private class StubSpawner : World.IWorldManager
		{
			private bool addPrefabCalled = false;
			private string addPrefabExpectedObjectId;
			private int addPrefabExpectedXPos;
			private int addPrefabExpectedZPos;
			private int addPrefabExpectedRot;

			private bool spawnMobCalled = false;
			private string spawnMobExpectedObjectId;
			private float spawnMobExpectedXPos;
			private float spawnMobExpectedZPos;
			private int spawnMobExpectedId;
			
			public void AddPrefab (string objectId, int xPos, int zPos, int rot)
			{
				addPrefabCalled = true;

				Assert.AreEqual (addPrefabExpectedObjectId, objectId);
				Assert.AreEqual (addPrefabExpectedXPos, xPos);
				Assert.AreEqual (addPrefabExpectedZPos, zPos);
				Assert.AreEqual (addPrefabExpectedRot, rot);
			}

			public void SetExpectedForAddPrefabCall (string objectId, int xPos, int zPos, int rot)
			{
				addPrefabExpectedObjectId = objectId;
				addPrefabExpectedXPos = xPos;
				addPrefabExpectedZPos = zPos;
				addPrefabExpectedRot = rot;
			}

			public bool WasAddPrefabCalled()
			{
				return addPrefabCalled;
			}

			public void SpawnMob (string objectId, float xPos, float zPos, int id)
			{
				spawnMobCalled = true;

				Assert.AreEqual (spawnMobExpectedObjectId, objectId);
				Assert.AreEqual (spawnMobExpectedXPos, xPos);
				Assert.AreEqual (spawnMobExpectedZPos, zPos);
				Assert.AreEqual (spawnMobExpectedId, id);
			}

		    public List<PlacedMob> GetMobs()
		    {
		        return null;
		    }

		    public void SetExpectedForSpawnMobCall (string objectId, float xPos, float zPos, int id)
			{
				spawnMobExpectedObjectId = objectId;
				spawnMobExpectedXPos = xPos;
				spawnMobExpectedZPos = zPos;
				spawnMobExpectedId = id;
			}

			public bool WasSpawnMobCalled()
			{
				return spawnMobCalled;
			}
		}
	}
}

