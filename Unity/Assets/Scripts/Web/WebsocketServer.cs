using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Parsing.Commands;

namespace Web
{
	class WebsocketServer : MonoBehaviour
	{
		public CommandResolver commandResolver;

		private int clientSocket = -1;
		private bool clientInitialised = false;


		void Start ()
		{
			NetworkTransport.Init ();

			ConnectionConfig config = new ConnectionConfig ();
			config.AddChannel (QosType.Reliable);

			HostTopology topology = new HostTopology (config, 2);

			clientSocket = NetworkTransport.AddWebsocketHost (topology, 9998);
			if (clientSocket < 0) {
				Debug.Log ("Client not set up");
			} else {
				Debug.Log ("Client set up");
			}
			clientInitialised = true;
		}

		void Update ()
		{
			if (!clientInitialised) {
				return;
			}

			int recHostId; 
			int connectionId;
			int channelId;
			int dataSize;
			byte[] buffer = new byte[1024];
			byte error;

			NetworkEventType networkEvent = NetworkEventType.DataEvent;
			do {
				networkEvent = NetworkTransport.Receive (out recHostId, out connectionId, out channelId, buffer, 1024, out dataSize, out error);

				switch (networkEvent) {
				case NetworkEventType.Nothing:
					break;
				case NetworkEventType.ConnectEvent:
					if (recHostId == clientSocket) {
						Debug.Log ("Client connected to " + connectionId.ToString () + "!");
					}
					break;

				case NetworkEventType.DataEvent:
					if (recHostId == clientSocket) {
						string msg = System.Text.Encoding.UTF8.GetString (buffer);
						commandResolver.ResolveMessage(msg);
					}
					break;

				case NetworkEventType.DisconnectEvent:
					if (recHostId == clientSocket) {
						Debug.Log ("Client has disconnected");
					}
					break;
				}

			} while (networkEvent != NetworkEventType.Nothing);
		}
	}
}