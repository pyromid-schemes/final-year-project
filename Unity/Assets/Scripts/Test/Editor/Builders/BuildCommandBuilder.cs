using System;

namespace Test.Builders
{
	public class BuildCommandBuilder
	{
		private static string template = "{{" +
			"\"command\":\"build\"," +
			"\"options\":{{" +
				"\"objectId\":\"{0}\"," +
				"\"xPos\":{1}," +
				"\"zPos\":{2}" +
			"}}" +
			"}}";
		
		private string objectId;
		private int xPos;
		private int zPos;

		public static BuildCommandBuilder ADefaultBuildCommandBuilder()
		{
			return new BuildCommandBuilder ()
				.WithObjectId ("default")
				.WithXPos (0)
				.WithZPos (0);
		}

		public string Build()
		{
			return String.Format (template, objectId, xPos, zPos);
		}

		public BuildCommandBuilder WithObjectId(string objectId)
		{
			this.objectId = objectId;
			return this;
		}

		public BuildCommandBuilder WithXPos(int xPos)
		{
			this.xPos = xPos;
			return this;
		}

		public BuildCommandBuilder WithZPos(int zPos)
		{
			this.zPos = zPos;
			return this;
		}
	}
}

