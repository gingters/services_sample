namespace Domain.Abstractions
{
	public interface IEventDispatcher
	{
		void DispatchEvent(IAggregateRoot aggregate, IEvent evt);
	}

	public interface IEventDispatcher<TAggregate> : IEventDispatcher
		where TAggregate : class, IAggregateRoot
	{
	}
}
