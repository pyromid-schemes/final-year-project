using System;
using NUnit.Framework;
using Communication;
using System.Collections.Generic;

namespace Test.Communication
{
	public class IPCManagerTest
	{
		private IPCManager underTest;

		public IPCManagerTest ()
		{
		}

		[SetUp]
		public void setup()
		{
			underTest = new IPCManager ();
		}

		[Test]
		public void ShouldRegisterEvent()
		{
			List<Event> expected = new List<Event> ();
			expected.Add (new StubEvent ());

			underTest.RegisterEvent (new StubEvent ());

			Assert.AreEqual (expected, underTest.ReceiveEventsForType (EventType.KillMob));
		}

		[Test]
		public void EventShouldBeRemovedAfterItIsReceived()
		{
			List<Event> expected = new List<Event> ();
			expected.Add (new StubEvent ());

			underTest.RegisterEvent (new StubEvent ());
			underTest.ReceiveEventsForType (EventType.KillMob);

			Assert.AreEqual (0, underTest.ReceiveEventsForType (EventType.KillMob).Count);

		}

		private class StubEvent : Event 
		{
			public EventType GetEventType()
			{
				return EventType.KillMob;
			}

			public override bool Equals(object value)
			{
				StubEvent inst = value as StubEvent;

				if (System.Object.ReferenceEquals(null, inst)) {
					return false;
				}

				return EventType.KillMob.Equals (inst.GetEventType());
			}
		}
	}
}

