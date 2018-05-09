using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions
{

	public interface IAggregateRoot
	{
		event EventHandler<EventRaisedEventArgs> EventRaised;
		int Version { get; }
		void ApplyEvent(IEvent evt);
	}
}
