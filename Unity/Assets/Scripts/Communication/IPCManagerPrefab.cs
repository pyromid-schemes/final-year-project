using System;
using UnityEngine;
/*
    @author Jamie Redding (jgr2)
*/

namespace Communication
{
	public class IPCManagerPrefab : MonoBehaviour
	{
		private IPCManager inner;

		void Start ()
		{
			inner = new IPCManager ();
		}

		public IPCManager GetIPCManager()
		{
			return inner;
		}
	}
}

