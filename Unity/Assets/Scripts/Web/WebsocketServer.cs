using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Parsing.Commands;
using World;
using System.Collections.Generic;
using System.Text;

namespace Web
{
	class WebsocketServer : MonoBehaviour
	{
		public WorldManager worldManager;
		public float updatesPerSecond = 30.0f;

		private int clientSocket = -1;
		private bool clientInitialised = false;
		private CommandResolver commandResolver;
		private WebsocketClient wsClient = null;
		private float nextUpdate = 0f;

		void Start ()
		{
			commandResolver = new CommandResolver (worldManager);

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

						byte[] gameWorld = ToByteArray(FormatGameWorldAsJson(worldManager.GetGameWorld()));
						NetworkTransport.Send(recHostId, connectionId, channelId, gameWorld, gameWorld.Length, out error); 
						wsClient = new WebsocketClient(recHostId, connectionId, channelId);
					}
					break;

				case NetworkEventType.DataEvent:
					if (recHostId == clientSocket) {
						commandResolver.ResolveMessage(FromByteArray (buffer));
					}
					break;

				case NetworkEventType.DisconnectEvent:
					if (recHostId == clientSocket) {
						Debug.Log ("Client has disconnected");
						wsClient = null;
					}
					break;
				}
					
			} while (networkEvent != NetworkEventType.Nothing);
		}

		void FixedUpdate()
		{
			if (wsClient != null) {
				float currentTime = Time.time;
				if (currentTime >= nextUpdate) {
					nextUpdate = currentTime + (1.0f/updatesPerSecond);
					SendVRPosition ();
				}
			}
		}

		void SendVRPosition()
		{
			byte[] position = ToByteArray(FormatVRPositionAsJson(worldManager.GetVRPosition ()));
			byte error;

			NetworkTransport.Send (wsClient.GetHostId(), wsClient.GetConnectionId(), wsClient.GetChannelId(), position, position.Length, out error);
		}

		private string FormatGameWorldAsJson(HashSet<PlacedPrefab> gameWorld)
		{
			StringBuilder sb = new StringBuilder ();
			sb.Append ("{");
			sb.Append ("\"command\":\"worldStatus\",");
			sb.Append ("\"objects\":[");

			foreach (PlacedPrefab p in gameWorld) {
				sb.Append ("{");
				sb.Append (string.Format("\"objectId\":\"{0}\",", p.GetName()));
				sb.Append (string.Format("\"xPos\":{0},", p.GetPosition().x));
				sb.Append (string.Format("\"zPos\":{0}", p.GetPosition().z));
				sb.Append ("},");
			}
			sb.Remove (sb.Length - 1, 1);
			sb.Append ("]}");

			return sb.ToString ();
		}

		private string FormatVRPositionAsJson(Vector3 position)
		{
			StringBuilder sb = new StringBuilder ();
			sb.Append ("{");
			sb.Append ("\"command\":\"vrPosition\",");
			sb.Append ("\"position\": {");
			sb.Append (string.Format("\"xPos\":{0},", position.x));
			sb.Append (string.Format("\"zPos\":{0}", position.z));
			sb.Append ("}}");

			return sb.ToString ();
		}

		private byte[] ToByteArray(string s)
		{
			return System.Text.Encoding.UTF8.GetBytes (s);
		}

		private string FromByteArray(byte[] b)
		{
			return System.Text.Encoding.UTF8.GetString (b);
		}
	}
}