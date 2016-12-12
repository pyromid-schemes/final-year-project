using System;
using UnityEngine;

namespace World
{
	public class AntiCorruption
	{
		public static float FixHandedness(float z)
		{
			return z * -1;
		}

		public static int FixHandedness(int z)
		{
			return z * -1;
		}
	}
}

