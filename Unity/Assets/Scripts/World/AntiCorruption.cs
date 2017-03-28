using System;
using UnityEngine;
/*
    @author Jamie Redding (jgr2)
*/

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

