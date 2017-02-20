using System;
using System.Collections.Generic;

namespace Communication
{
	public class IPCManager
	{
		private List<Event> events;

		public IPCManager ()
		{
			events = new List<Event> ();
		}

		public List<Event> ReceiveEventsForType(EventType type)
		{
			var ret = events.FindAll (e => e.GetEventType ().Equals (type));
			events.RemoveAll (e => e.GetEventType ().Equals (type));
			return ret;
		}

		public void RegisterEvent (Event e)
		{
			events.Add (e);
		}
	}
}

