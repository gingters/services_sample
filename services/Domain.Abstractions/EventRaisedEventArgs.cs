using System;

namespace Domain.Abstractions
{
	public class EventRaisedEventArgs : EventArgs
	{
		public IEvent Event { get; private set; }

		public EventRaisedEventArgs(IEvent evt)
		{
			Event = evt;
		}
	}
}
