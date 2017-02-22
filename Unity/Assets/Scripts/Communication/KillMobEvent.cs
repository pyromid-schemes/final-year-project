using System;
using World;

namespace Communication
{
	public class KillMobEvent : Event
	{
		private PlacedMob mob;

		public KillMobEvent (PlacedMob mob)
		{
			this.mob = mob;
		}

		public EventType GetEventType()
		{
			return EventType.KillMob;
		}

		public PlacedMob GetMob()
		{
			return mob;
		}
	}
}

