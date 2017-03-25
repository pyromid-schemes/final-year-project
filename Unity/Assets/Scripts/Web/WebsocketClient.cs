using System;
/*
    @author Jamie Redding (jgr2)
*/

namespace Web
{
	public class WebsocketClient
	{
		private int hostId;
		private int connectionId;
		private int channelId;

		public WebsocketClient (int hostId, int connectionId, int channelId)
		{
			this.hostId = hostId;
			this.connectionId = connectionId;
			this.channelId = channelId;
		}

		public int GetHostId()
		{
			return hostId;
		}

		public int GetConnectionId()
		{
			return connectionId;
		}

		public int GetChannelId()
		{
			return channelId;
		}
	}
}

