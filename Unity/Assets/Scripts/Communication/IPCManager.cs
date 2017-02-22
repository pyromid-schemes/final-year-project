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
			List<Event> eventsForType = new List<Event> ();
			for (int i = events.Count - 1; i >= 0; i--) {
				if (events [i].GetEventType ().Equals (type)) {
					eventsForType.Add (events [i]);
					events.RemoveAt (i);
				}
			}
			return eventsForType;
		}

		public void RegisterEvent (Event e)
		{
			events.Add (e);
		}
	}
}

