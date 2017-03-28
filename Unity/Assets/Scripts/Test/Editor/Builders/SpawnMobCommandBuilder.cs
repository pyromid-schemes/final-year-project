using System;
/**
 * @author Jamie Redding
 */
namespace Test.Builders
{
	public class SpawnMobCommandBuilder
	{
		private static string template = "{{" +
			"\"command\":\"spawnMob\"," +
			"\"options\":{{" +
				"\"objectId\":\"{0}\"," +
				"\"xPos\":{1}," +
				"\"zPos\":{2}," +
				"\"id\":{3}" +
			"}}" +
			"}}";
		
		private string objectId;
		private float xPos;
		private float zPos;
		private int id;

		public static SpawnMobCommandBuilder ADefaultSpawnMobCommand()
		{
			return new SpawnMobCommandBuilder ()
				.WithObjectId ("default")
				.WithXPos (0f)
				.WithZPos (0f)
				.WithId (0);
		}

		public string Build()
		{
			return String.Format (template, objectId, xPos, zPos, id);
		}

		public SpawnMobCommandBuilder WithObjectId(string objectId)
		{
			this.objectId = objectId;
			return this;
		}

		public SpawnMobCommandBuilder WithXPos(float xPos)
		{
			this.xPos = xPos;
			return this;
		}

		public SpawnMobCommandBuilder WithZPos(float zPos)
		{
			this.zPos = zPos;
			return this;
		}

		public SpawnMobCommandBuilder WithId(int id)
		{
			this.id = id;
			return this;
		}
	}
}

