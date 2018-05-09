using System;

namespace Domain.Abstractions
{
	public interface IEvent
	{
		int AggregateId { get; }
		int Id { get; set; }
		string Type { get; }
		int Version { get; }
		DateTime TimeStamp { get; }
	}
}
