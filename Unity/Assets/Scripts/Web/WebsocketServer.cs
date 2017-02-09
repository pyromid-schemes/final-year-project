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

		private int clientSocket = -1;
		private bool clientInitialised = false;
		private CommandResolver commandResolver;
		private WebsocketClient wsClient = null;

		private static float updatesPerSecond = 30.0f;
		private float TIMER_TICK = (1.0f / updatesPerSecond);
		private float newTime = 0f;


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
					string data = FromByteArray(buffer);
					Debug.Log (data);

					if (recHostId == clientSocket) {
						commandResolver.ResolveMessage(data);
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
				newTime += Time.deltaTime;

				if (newTime >= TIMER_TICK) {
					newTime -= TIMER_TICK;

					SendPositions ();
				}
			}
		}

		void SendPositions()
		{
			byte[] position = ToByteArray(FormatPositionsAsJson (worldManager.GetVRPosition (), worldManager.GetMobs ()));
			byte error;

			NetworkTransport.Send (wsClient.GetHostId(), wsClient.GetConnectionId(), wsClient.GetChannelId(), position, position.Length, out error);
		}

		private string FormatGameWorldAsJson(List<PlacedPrefab> gameWorld)
		{
			StringBuilder sb = new StringBuilder ();
			sb.Append ("{");
			sb.Append ("\"command\":\"worldStatus\",");
			sb.Append ("\"objects\":[");

			foreach (PlacedPrefab p in gameWorld) {
				sb.Append ("{");
				sb.Append (string.Format("\"objectId\":\"{0}\",", p.GetName()));
				sb.Append (string.Format("\"xPos\":{0},", p.GetPosition().x));
				sb.Append (string.Format("\"zPos\":{0}", AntiCorruption.FixHandedness(p.GetPosition().z)));
				sb.Append ("},");
			}
			sb.Remove (sb.Length - 1, 1);
			sb.Append ("]}");

			return sb.ToString ();
		}

		private string FormatPositionsAsJson(Vector3 position, List<PlacedMob> mobs)
		{
			StringBuilder sb = new StringBuilder ();
			sb.Append ("{");
			sb.Append ("\"command\":\"positions\",");
			sb.Append ("\"vrPosition\": {");
			sb.Append (string.Format("\"xPos\":{0},", position.x));
			sb.Append (string.Format("\"zPos\":{0}",AntiCorruption.FixHandedness(position.z)));
			sb.Append ("},");
			sb.Append ("\"mobs\":[");

			foreach (PlacedMob m in mobs) {
				sb.Append ("{");
				sb.Append (string.Format("\"objectId\":\"{0}\",", m.GetName ()));
				sb.Append (string.Format("\"xPos\":{0},", m.GetGameObject().transform.position.x));
				sb.Append (string.Format("\"zPos\":{0},", AntiCorruption.FixHandedness(m.GetGameObject ().transform.position.z)));

				bool killMob = ((IDamageable)m.GetGameObject ().GetComponent (typeof(IDamageable))).IsDead ();

				if (killMob) {
					m.KillMob ();
				}

				sb.Append (string.Format("\"dead\":{0},", killMob ? "true" : "false"));
				sb.Append (string.Format("\"id\":{0}", m.GetId ()));
				sb.Append ("},");
			}
			if (mobs.Count > 0) {
				sb.Remove (sb.Length - 1, 1);
			}
			sb.Append ("]}");

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