using System;
using System.Collections.Generic;
using System.Text;
using Domain.Abstractions;

namespace Domain.Services
{
	public abstract class Event : IEvent
	{
		// TODO: Event should be immutable. Set this in ctor
		public int AggregateId { get; set; }
		public int Id { get; set; }
		public string Type { get; set; }
		public int Version { get; set; }
		public DateTime TimeStamp { get; set; }
	}
}
